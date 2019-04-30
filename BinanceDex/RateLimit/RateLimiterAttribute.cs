/*
 *
 * Kicking this can down the road for now.
 *
 *
 *
 */

using System;

namespace BinanceDex.RateLimit
{
    [AttributeUsage(AttributeTargets.Method)]
    public class RateLimiterAttribute : Attribute
    {
        #region Properties

        public int PerTimeInSeconds { get; set; }
        public int Rate { get; set; }

        #endregion
    }
}