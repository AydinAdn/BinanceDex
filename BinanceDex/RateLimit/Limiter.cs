using System;
using System.Threading;
using System.Threading.Tasks;

namespace BinanceDex.RateLimit
{
    public class Limiter
    {
        private SemaphoreSlim slim;
        public Limiter(int rate, TimeSpan minimumDelay)
        {
            this.Rate = rate;
            this.TimesUsed = new DateTime[rate];
            this.MinimumDelay = minimumDelay;
            this.slim = new SemaphoreSlim(1,1);
        }

        public int Rate { get; set; }
        public TimeSpan MinimumDelay { get; set; }
        public DateTime[] TimesUsed { get; set; }

        public async Task<T> Try<T>(Func<Task<T>> func)
        {
            while (true)
            {
                for (int i = 0; i < this.TimesUsed.Length; i++)
                {
                    await this.slim.WaitAsync();

                    if (this.TimesUsed[i] >= DateTime.UtcNow.AddSeconds(-this.MinimumDelay.Seconds * 1.1))
                    {
                        this.slim.Release();
                        continue;
                    }

                    T result = await func.Invoke();

                    this.TimesUsed[i] = DateTime.UtcNow;

                    this.slim.Release();
                    return result;
                }

                await Task.Delay(100);
            }
        }

        public async Task<T> Try<T>(Func<Task<T>> func, CancellationToken token)
        {
            while (true)
            {
                for (int i = 0; i < this.TimesUsed.Length; i++)
                {
                    await this.slim.WaitAsync(token);

                    if (this.TimesUsed[i] >= DateTime.UtcNow.AddSeconds(-this.MinimumDelay.Seconds * 1.1))
                    {
                        this.slim.Release();
                        continue;
                    }

                    T result = await func.Invoke();

                    this.TimesUsed[i] = DateTime.UtcNow;

                    this.slim.Release();
                    return result;
                }

                await Task.Delay(100, token);
            }
        }
    }
}