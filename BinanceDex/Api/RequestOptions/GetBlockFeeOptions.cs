namespace BinanceDex.Api.RequestOptions
{
    public class GetBlockFeeOptions : OptionsBase
    {
        public string Address { get; set; }
        public long? End { get; set; }
        public int? Limit { get; set; }
        public int? Offset { get; set; }
        public long? Start { get; set; }
        public int? Total { get; set; }
    }
}