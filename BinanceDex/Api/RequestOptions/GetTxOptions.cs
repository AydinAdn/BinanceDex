namespace BinanceDex.Api.RequestOptions
{
    public class GetTxOptions : OptionsBase
    {
        public string Address { get; set; }
        public long? BlockHeight { get; set; }
        public long? EndTime { get; set; }
        public int? Limit { get; set; }
        public int? Offset { get; set; }
        public TxSide? Side { get; set; }
        public string TxAsset { get; set; }
        public TxType? TxType { get; set; }
    }
}