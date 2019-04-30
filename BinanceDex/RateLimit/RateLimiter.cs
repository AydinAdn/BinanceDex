using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;
using BinanceDex.Utilities;

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

        public RateLimitInfo GetRateLimiter(string endpoint)
        {
            Throw.IfNullOrWhiteSpace(endpoint, nameof(endpoint));
            return this.limits[endpoint];
        }

        //public async Task GetLimiter([CallerMemberName] string caller)

    }
}