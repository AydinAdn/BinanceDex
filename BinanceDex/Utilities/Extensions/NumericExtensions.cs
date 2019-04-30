using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinanceDex.Utilities.Extensions
{
    public static class NumericExtensions
    {
        public static DateTime ToDateTime(this long unixTime)
        {
            return epoch.AddMilliseconds((long) unixTime);
        }

        private static readonly DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
    }
}
