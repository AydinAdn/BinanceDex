using System.Collections.Generic;
using Newtonsoft.Json;

namespace BinanceDex.Api.Models
{
    public class MarketDepth : ApiError
    {
        #region Properties

        [JsonProperty("asks")]
        public IList<OrderBookPriceLevel> Asks { get; set; }

        [JsonProperty("bids")]
        public IList<OrderBookPriceLevel> Bids { get; set; }

        [JsonProperty("height")]
        public int Height { get; set; }

        #endregion
    }
}