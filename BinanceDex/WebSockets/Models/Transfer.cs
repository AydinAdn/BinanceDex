using System.Collections.Generic;
using Newtonsoft.Json;

namespace BinanceDex.WebSockets.Models
{
    public class Transfer
    {

        [JsonProperty("o")]
        public string ToAddress { get; set; }

        [JsonProperty("c")]
        public IList<Coin> Coins { get; set; }
    }
}