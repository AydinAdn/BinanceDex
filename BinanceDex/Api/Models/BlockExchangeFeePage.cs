using Newtonsoft.Json;
using System.Collections.Generic;

namespace BinanceDex.Api.Models
{
    public class BlockExchangeFee
    {
        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("blockHeight")]
        public int BlockHeight { get; set; }

        [JsonProperty("blockTime")]
        public object BlockTime { get; set; }

        [JsonProperty("fee")]
        public string Fee { get; set; }

        [JsonProperty("tradeCount")]
        public int TradeCount { get; set; }
    }

    public class BlockExchangeFeePage
    {
        [JsonProperty("blockExchangeFee")]
        public IList<BlockExchangeFee> BlockExchangeFee { get; set; }

        [JsonProperty("total")]
        public int Total { get; set; }
    }
}
