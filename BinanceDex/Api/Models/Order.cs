using Newtonsoft.Json;

namespace BinanceDex.Api.Models
{
    public class Order
    {
        #region Properties

        [JsonProperty("cumulateQuantity")]
        public string CumulateQuantity { get; set; }

        [JsonProperty("fee")]
        public string Fee { get; set; }

        [JsonProperty("lastExecutedPrice")]
        public string LastExecutedPrice { get; set; }

        [JsonProperty("lastExecutedQuantity")]
        public string LastExecutedQuantity { get; set; }

        [JsonProperty("orderCreateTime")]
        public string OrderCreateTime { get; set; }

        [JsonProperty("orderId")]
        public string OrderId { get; set; }

        [JsonProperty("owner")]
        public string Owner { get; set; }

        [JsonProperty("price")]
        public string Price { get; set; }

        [JsonProperty("quantity")]
        public string Quantity { get; set; }

        [JsonProperty("side")]
        public OrderSide Side { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("symbol")]
        public string Symbol { get; set; }

        [JsonProperty("timeInForce")]
        public int TimeInForce { get; set; }

        [JsonProperty("tradeId")]
        public string TradeId { get; set; }

        [JsonProperty("transactionHash")]
        public string TransactionHash { get; set; }

        [JsonProperty("transactionTime")]
        public string TransactionTime { get; set; }

        [JsonProperty("type")]
        public int Type { get; set; }

        #endregion
    }
}