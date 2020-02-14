using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using BinanceDex.Utilities;

namespace BinanceDex.RateLimit
{
    public interface IRateLimiter
    {
        Limiter GetRateLimiter([CallerMemberName] string endpoint = "");
    }

    public class RateLimiter<T> : IRateLimiter
    {
        private readonly ConcurrentDictionary<string, Limiter> limits;

        public RateLimiter()
        {
            this.limits = new ConcurrentDictionary<string, Limiter>();

            typeof(T).GetMethods()
                     .Select(x => new { x.Name, Limit = x.GetCustomAttribute<RateLimiterAttribute>() })
                     .Where(x=>x.Name.Contains("Hrp") == false)
                     .ToList()
                     .ForEach(x =>
                     {
                         this.limits.GetOrAdd(x.Name, new Limiter(x.Limit.Rate, TimeSpan.FromSeconds(x.Limit.PerTimeInSeconds)));
                     });
        }


        public Limiter GetRateLimiter([CallerMemberName] string endpoint = "")
        {
            Throw.IfNull(endpoint, nameof(endpoint));
            return this.limits[endpoint];
        }
    }
}