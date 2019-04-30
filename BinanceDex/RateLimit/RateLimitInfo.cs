using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace BinanceDex.RateLimit
{
    public class RateLimitInfo
    {
        private object locker;
        private SemaphoreSlim slim;
        public RateLimitInfo(int rate, TimeSpan minimumDelay)
        {
            this.Rate = rate;
            this.TimesUsed = new DateTime[rate];
            this.MinimumDelay = minimumDelay;
            this.locker = new object();
            this.slim = new SemaphoreSlim(1,1);
        }

        public int Rate { get; set; }
        public TimeSpan MinimumDelay { get; set; }
        public DateTime[] TimesUsed { get; set; }

        public int CheckLimit()
        {
            DateTime minimum = DateTime.UtcNow.AddSeconds(-this.MinimumDelay.Seconds);

            int available = this.TimesUsed.Count(x => x < minimum);

            return available;
        }

        public async Task<T> Try<T>(Func<Task<T>> func, CancellationToken token)
        {
            bool canExit = false;
            while (true)
            {
                for (int i = 0; i < this.TimesUsed.Length; i++)
                {
                    T result;
                    await this.slim.WaitAsync(token);

                    if (this.TimesUsed[i] >= DateTime.UtcNow.AddSeconds(-this.MinimumDelay.Seconds * 1.1))
                    {
                        this.slim.Release();
                        continue;
                    }

                    result = await func.Invoke();

                    this.TimesUsed[i] = DateTime.UtcNow;

                    this.slim.Release();
                    return result;
                }

                await Task.Delay(100, token);
            }
        }
    }
}