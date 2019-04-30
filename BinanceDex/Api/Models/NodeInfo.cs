using Newtonsoft.Json;

namespace BinanceDex.Api.Models
{
    public class NodeInfo
    {
        #region Properties

        [JsonProperty("channels")]
        public string Channels { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("listen_addr")]
        public string ListenAddr { get; set; }

        [JsonProperty("moniker")]
        public string Moniker { get; set; }

        [JsonProperty("network")]
        public string Network { get; set; }

        [JsonProperty("other")]
        public NodeInfoOther NodeInfoOther { get; set; }

        [JsonProperty("version")]
        public string Version { get; set; }

        #endregion
    }
}