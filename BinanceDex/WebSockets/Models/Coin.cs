using Newtonsoft.Json;

namespace BinanceDex.WebSockets.Models
{
    public class Coin
    {

        [JsonProperty("a")]
        public string Asset { get; set; }

        [JsonProperty("A")]
        public decimal Amount { get; set; }
    }
}