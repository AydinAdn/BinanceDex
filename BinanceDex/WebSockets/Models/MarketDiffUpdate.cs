using System;
using System.Collections.Generic;
using BinanceDex.Api.Models;
using Newtonsoft.Json;

namespace BinanceDex.WebSockets.Models
{

    public class MarketDiffUpdate  : EventArgs
    {

        [JsonProperty("e")]
        public string EventType { get; set; }

        [JsonProperty("E")]
        public long EventHeight { get; set; }

        [JsonProperty("s")]
        public string Symbol { get; set; }

        [JsonProperty("b")]
        public IList<OrderBookPriceLevel> Bids { get; set; }

        [JsonProperty("a")]
        public IList<OrderBookPriceLevel> Asks { get; set; }
    }
}