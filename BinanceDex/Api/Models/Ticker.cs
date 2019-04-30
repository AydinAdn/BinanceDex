using Newtonsoft.Json;

namespace BinanceDex.Api.Models
{
    public class Ticker
    {
        #region Properties

        [JsonProperty("askPrice")]
        public decimal AskPrice { get; set; }

        [JsonProperty("askQuantity")]
        public decimal AskQuantity { get; set; }

        [JsonProperty("bidPrice")]
        public decimal BidPrice { get; set; }

        [JsonProperty("bidQuantity")]
        public decimal BidQuantity { get; set; }

        [JsonProperty("closeTime")]
        public long CloseTime { get; set; }

        [JsonProperty("count")]
        public int Count { get; set; }

        [JsonProperty("firstId")]
        public string FirstId { get; set; }

        [JsonProperty("highPrice")]
        public decimal HighPrice { get; set; }

        [JsonProperty("lastId")]
        public string LastId { get; set; }

        [JsonProperty("lastPrice")]
        public decimal LastPrice { get; set; }

        [JsonProperty("lastQuantity")]
        public decimal LastQuantity { get; set; }

        [JsonProperty("lowPrice")]
        public decimal LowPrice { get; set; }

        [JsonProperty("openPrice")]
        public decimal OpenPrice { get; set; }

        [JsonProperty("openTime")]
        public long OpenTime { get; set; }

        [JsonProperty("prevClosePrice")]
        public decimal PrevClosePrice { get; set; }

        [JsonProperty("priceChange")]
        public decimal PriceChange { get; set; }

        [JsonProperty("priceChangePercent")]
        public decimal PriceChangePercent { get; set; }

        [JsonProperty("quoteVolume")]
        public decimal QuoteVolume { get; set; }

        [JsonProperty("symbol")]
        public string Symbol { get; set; }

        [JsonProperty("volume")]
        public decimal Volume { get; set; }

        [JsonProperty("weightedAvgPrice")]
        public decimal WeightedAvgPrice { get; set; }

        #endregion
    }
}