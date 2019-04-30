using Newtonsoft.Json;

namespace BinanceDex.Wallet
{
    public class WalletInfo
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("pub_key")]
        public string PubKey { get; set; }

        [JsonProperty("seed")]
        public string Seed { get; set; }
    }
}