using System.Diagnostics.CodeAnalysis;

namespace BinanceDex.Api.Models
{
    [SuppressMessage("ReSharper", "InconsistentNaming")]
    public enum CandleStickInterval
    {
        [Descriptor("1m")] Minutes_1,

        [Descriptor("3m")] Minutes_3,

        [Descriptor("5m")] Minutes_5,

        [Descriptor("15m")] Minutes_15,

        [Descriptor("30m")] Minutes_30,

        [Descriptor("1h")] Hours_1,

        [Descriptor("2h")] Hours_2,

        [Descriptor("4h")] Hours_4,

        [Descriptor("6h")] Hours_6,

        [Descriptor("8h")] Hours_8,

        [Descriptor("12h")] Hours_12,

        [Descriptor("1d")] Days_1,

        [Descriptor("3d")] Days_3,

        [Descriptor("1w")] Weeks_1,

        [Descriptor("1M")] Months_1
    }
}