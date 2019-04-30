using System.Collections.Generic;
using Newtonsoft.Json;

namespace BinanceDex.WebSockets.Models
{
    public class AccountUpdate
    {

        [JsonProperty("e")]
        public string EventType { get; set; }

        [JsonProperty("E")]
        public long EventHeight { get; set; }

        [JsonProperty("B")]
        public IList<Balance> Balances { get; set; }
    }
}