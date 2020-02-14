using System;

namespace BinanceDex.Utilities.Extensions
{
    public static class NumericExtensions
    {
        public static DateTime ToDateTime(this long unixTime)
        {
            return Epoch.AddMilliseconds(unixTime);
        }

        private static readonly DateTime Epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
    }
}
