using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;

namespace BinanceDex.RateLimit
{
    public class RateLimiter<T>
    {
        private ConcurrentDictionary<string, RateLimitInfo> limits;

        public RateLimiter()
        {
            this.limits = new ConcurrentDictionary<string, RateLimitInfo>();

            typeof(T).GetMethods()
                     .Select(x => new { Name = x.Name, Limit = x.GetCustomAttribute<RateLimiterAttribute>() })
                     .ToList()
                     .ForEach(x =>
                     {
                         this.limits.GetOrAdd(x.Name, new RateLimitInfo(x.Limit.Rate, TimeSpan.FromSeconds(x.Limit.PerTimeInSeconds)));
                     });
        }

        // TODO: Implement async method to retrieve the correct limiter from the dictionary.


        //public async Task GetLimiter([CallerMemberName] string caller)

    }
}