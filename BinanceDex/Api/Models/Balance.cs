using Newtonsoft.Json;

namespace BinanceDex.Api.Models
{
    public class Balance : ApiError
    {
        #region Properties

        [JsonProperty("free")]
        public string Free { get; set; }

        [JsonProperty("frozen")]
        public string Frozen { get; set; }

        [JsonProperty("locked")]
        public string Locked { get; set; }

        [JsonProperty("symbol")]
        public string Symbol { get; set; }

        #endregion
    }
}