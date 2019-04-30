using Newtonsoft.Json;

namespace BinanceDex.Api.Models
{
    public class Node : ApiError
    {
        #region Properties

        [JsonProperty("node_info")]
        public NodeInfo NodeInfo { get; set; }

        [JsonProperty("sync_info")]
        public SyncInfo SyncInfo { get; set; }

        [JsonProperty("validator_info")]
        public ValidatorInfo ValidatorInfo { get; set; }

        #endregion
    }
}