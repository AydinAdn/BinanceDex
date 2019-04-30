using Newtonsoft.Json;

namespace BinanceDex.WebSockets.Models
{
    public class Balance
    {

        [JsonProperty("a")]
        public string Asset { get; set; }

        [JsonProperty("f")]
        public decimal Free { get; set; }

        [JsonProperty("l")]
        public decimal Locked { get; set; }

        [JsonProperty("r")]
        public decimal Frozen { get; set; }
    }
}