using System.Collections.Generic;
using Newtonsoft.Json;

namespace BinanceDex.Api.Models
{
    public class Peer : ApiError
    {
        #region Properties

        [JsonProperty("accelerated")]
        public bool Accelerated { get; set; }

        [JsonProperty("access_addr")]
        public string AccessAddr { get; set; }

        [JsonProperty("capabilities")]
        public IList<string> Capabilities { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("listen_addr")]
        public string ListenAddr { get; set; }

        [JsonProperty("moniker")]
        public string Moniker { get; set; }

        [JsonProperty("network")]
        public string Network { get; set; }

        [JsonProperty("original_listen_addr")]
        public string OriginalListenAddr { get; set; }

        [JsonProperty("stream_addr")]
        public string StreamAddr { get; set; }

        [JsonProperty("version")]
        public string Version { get; set; }

        #endregion
    }
}