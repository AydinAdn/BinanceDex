using System.Collections.Generic;
using Newtonsoft.Json;

namespace BinanceDex.WebSockets.Models
{
    public class TransferUpdate
    {

        [JsonProperty("e")]
        public string EventType { get; set; }

        [JsonProperty("E")]
        public int EventHeight { get; set; }

        [JsonProperty("H")]
        public string TransactionHash { get; set; }

        [JsonProperty("f")]
        public string FromAddress { get; set; }

        [JsonProperty("t")]
        public IList<Transfer> Transfers { get; set; }
    }
}