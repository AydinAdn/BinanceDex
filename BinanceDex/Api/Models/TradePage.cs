using System.Collections.Generic;
using Newtonsoft.Json;

namespace BinanceDex.Api.Models
{
    public class Trade
    {
        #region Properties

        [JsonProperty("baseAsset")]
        public string BaseAsset { get; set; }

        [JsonProperty("blockHeight")]
        public int BlockHeight { get; set; }

        [JsonProperty("buyerId")]
        public string BuyerId { get; set; }

        [JsonProperty("buyerOrderId")]
        public string BuyerOrderId { get; set; }

        [JsonProperty("buyFee")]
        public string BuyFee { get; set; }

        [JsonProperty("price")]
        public string Price { get; set; }

        [JsonProperty("quantity")]
        public string Quantity { get; set; }

        [JsonProperty("quoteAsset")]
        public string QuoteAsset { get; set; }

        [JsonProperty("sellerId")]
        public string SellerId { get; set; }

        [JsonProperty("sellerOrderId")]
        public string SellerOrderId { get; set; }

        [JsonProperty("sellFee")]
        public string SellFee { get; set; }

        [JsonProperty("symbol")]
        public string Symbol { get; set; }

        [JsonProperty("time")]
        public object Time { get; set; }

        [JsonProperty("tradeId")]
        public string TradeId { get; set; }

        #endregion
    }

    public class TradePage
    {
        #region Properties

        [JsonProperty("total")]
        public int Total { get; set; }

        [JsonProperty("trade")]
        public IList<Trade> Trade { get; set; }

        #endregion
    }
}