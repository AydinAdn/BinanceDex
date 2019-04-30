using System;
using Newtonsoft.Json;

namespace BinanceDex.Api.Models
{
    public class SyncInfo
    {
        #region Properties

        [JsonProperty("catching_up")]
        public bool CatchingUp { get; set; }

        [JsonProperty("latest_app_hash")]
        public string LatestAppHash { get; set; }

        [JsonProperty("latest_block_hash")]
        public string LatestBlockHash { get; set; }

        [JsonProperty("latest_block_height")]
        public long LatestBlockHeight { get; set; }

        [JsonProperty("latest_block_time")]
        public DateTime LatestBlockTime { get; set; }

        #endregion
    }
}