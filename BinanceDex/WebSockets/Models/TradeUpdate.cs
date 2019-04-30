using Newtonsoft.Json;

namespace BinanceDex.WebSockets.Models
{
    public class TradeUpdate
    {

        [JsonProperty("e")]
        public string EventType { get; set; }

        [JsonProperty("E")]
        public long EventHeight { get; set; }

        [JsonProperty("s")]
        public string Symbol { get; set; }

        [JsonProperty("t")]
        public string TradeId { get; set; }

        [JsonProperty("p")]
        public decimal Price { get; set; }

        [JsonProperty("q")]
        public decimal Quantity { get; set; }

        [JsonProperty("b")]
        public string BuyerOrderId { get; set; }

        [JsonProperty("a")]
        public string SellerOrderId { get; set; }

        [JsonProperty("T")]
        public long TradeTime { get; set; }

        [JsonProperty("sa")]
        public string SellerAddress { get; set; }

        [JsonProperty("ba")]
        public string BuyerAddress { get; set; }
    }
}