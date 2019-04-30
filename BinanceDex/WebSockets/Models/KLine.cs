using Newtonsoft.Json;

namespace BinanceDex.WebSockets.Models
{
    public class KLine
    {

        [JsonProperty("t")]
        public long OpenTime { get; set; }

        [JsonProperty("T")]
        public long CloseTime { get; set; }

        [JsonProperty("s")]
        public string Symbol { get; set; }

        [JsonProperty("i")]
        public string Interval { get; set; }

        [JsonProperty("f")]
        public string FirstTradeId { get; set; }

        [JsonProperty("L")]
        public string LastTradeId { get; set; }

        [JsonProperty("o")]
        public decimal Open { get; set; }

        [JsonProperty("c")]
        public decimal Close { get; set; }

        [JsonProperty("h")]
        public decimal High { get; set; }

        [JsonProperty("l")]
        public decimal Low { get; set; }

        [JsonProperty("v")]
        public decimal Volume { get; set; }

        [JsonProperty("n")]
        public int NumberOfTrades { get; set; }

        [JsonProperty("x")]
        public bool IsKlineClosed { get; set; }

        [JsonProperty("q")]
        public decimal QuoteAssetVolume { get; set; }
    }
}