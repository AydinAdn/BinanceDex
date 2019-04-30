using Newtonsoft.Json;

namespace BinanceDex.Api.Models
{
    public class Transaction : ApiError
    {
        #region Properties

        [JsonProperty("data")]
        public string Data { get; set; }

        [JsonProperty("hash")]
        public string Hash { get; set; }

        [JsonProperty("log")]
        public string Log { get; set; }

        [JsonProperty("ok")]
        public bool Ok { get; set; }

        #endregion
    }
}