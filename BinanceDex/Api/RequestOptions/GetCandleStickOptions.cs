using BinanceDex.Api.Models;

namespace BinanceDex.Api.RequestOptions
{
    public class GetCandleStickOptions : OptionsBase
    {
        public string Symbol { get; set; }
        public CandleStickInterval Interval { get; set; }
        public int? Limit { get; set; }
        public long? Start { get; set; }
        public long? End { get; set; }
    }
}