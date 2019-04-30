using BinanceDex.Api.Models;
using Newtonsoft.Json;

namespace BinanceDex.WebSockets.Models
{
    public class OrderUpdate
    {
     
        [JsonProperty("e")]
        public string EventType { get; set; }
     
        [JsonProperty("E")]
        public long EventHeight { get; set; }
     
        [JsonProperty("s")]
        public string Symbol { get; set; }
     
        [JsonProperty("S")]
        public OrderSide Side { get; set; }
     
        [JsonProperty("o")]
        public int OrderType { get; set; }
     
        [JsonProperty("f")]
        public TimeInForce TimeInForce { get; set; }
     
        [JsonProperty("q")]
        public decimal OrderQuantity { get; set; }
     
        [JsonProperty("p")]
        public decimal OrderPrice { get; set; }
     
        [JsonProperty("x")]
        public string CurrentExecutionType { get; set; }
     
        [JsonProperty("X")]
        public string CurrentOrderStatus { get; set; }
     
        [JsonProperty("i")]
        public string OrderId { get; set; }
     
        [JsonProperty("l")]
        public decimal LastExecutedQuantity { get; set; }
     
        [JsonProperty("z")]
        public decimal CumulativeFilledQuantity { get; set; }
     
        [JsonProperty("L")]
        public decimal LastExecutedPrice { get; set; }
     
        [JsonProperty("n")]
        public string Commissions { get; set; }
     
        [JsonProperty("T")]
        public long TransactionTime { get; set; }
     
        [JsonProperty("t")]
        public string TradeId { get; set; }
     
        [JsonProperty("O")]
        public long CreationTime { get; set; }
    }
}