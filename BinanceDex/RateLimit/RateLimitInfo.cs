// TODO: Currently kind of works in principle, not in use at the moment, mocked together as proof of concept.

using System;
using System.Linq;

namespace BinanceDex.RateLimit
{
    public class RateLimitInfo
    {
        public RateLimitInfo(int rate, TimeSpan minimumDelay)
        {
            this.TimesUsed = new DateTime[rate];
            this.MinimumDelay = minimumDelay;
        }

        public int Rate { get; set; }
        public TimeSpan MinimumDelay { get; set; }
        public DateTime[] TimesUsed { get; set; }

        public int CheckLimit()
        {
            var minimum = DateTime.UtcNow.AddSeconds(-this.MinimumDelay.Seconds);

            var available = this.TimesUsed.Count(x => x < minimum);

            return available;
        }

        public void Hit()
        {
            bool canContinue = false;
            while (!canContinue)
            {
                //            Console.WriteLine("-");
                for (int i = 0; i < this.TimesUsed.Length; i++)
                {
                    if (this.TimesUsed[i] < DateTime.UtcNow.AddSeconds(-this.MinimumDelay.Seconds))
                    {
                        this.TimesUsed[i] = DateTime.UtcNow;
                        canContinue = true;
                        break;
                    }
                }
                //            Task.Delay(100).Wait();
            }

        }
    }
}