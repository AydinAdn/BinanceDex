using MessagePack;

namespace BinanceDex.Api.Models
{
    [MessagePackObject]
    public class CandleStick
    {
        #region Properties

        [Key(0)]
        public long OpenTime { get; set; }

        [Key(1)]
        public decimal Open { get; set; }

        [Key(2)]
        public decimal High { get; set; }

        [Key(3)]
        public decimal Low { get; set; }

        [Key(4)]
        public decimal Close { get; set; }

        [Key(5)]
        public decimal Volume { get; set; }

        [Key(6)]
        public long CloseTime { get; set; }

        [Key(7)]
        public decimal QuoteAssetVolume { get; set; }

        [Key(8)]
        public int NumberOfTrades { get; set; }

        #endregion
    }
}