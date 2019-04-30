using Newtonsoft.Json;

namespace BinanceDex.Api.Models
{
    public class Market
    {
        #region Properties

        [JsonProperty("base_asset_symbol")]
        public string BaseAssetSymbol { get; set; }

        [JsonProperty("list_price")]
        public decimal ListPrice { get; set; }

        [JsonProperty("lot_size")]
        public decimal LotSize { get; set; }

        [JsonProperty("quote_asset_symbol")]
        public string QuoteAssetSymbol { get; set; }

        [JsonProperty("tick_size")]
        public decimal TickSize { get; set; }

        #endregion
    }
}