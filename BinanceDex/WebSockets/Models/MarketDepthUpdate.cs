using System.Collections.Generic;
using BinanceDex.Api.Models;
using Newtonsoft.Json;

namespace BinanceDex.WebSockets.Models
{
    public class MarketDepthUpdate
    {
        [JsonProperty("lastUpdateId")]
        public int LastUpdateId { get; set; }

        [JsonProperty("symbol")]
        public string Symbol { get; set; }

        [JsonProperty("bids")]
        public IList<OrderBookPriceLevel> Bids { get; set; }

        [JsonProperty("asks")]
        public IList<OrderBookPriceLevel> Asks { get; set; }
    }
}