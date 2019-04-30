using System;
using Newtonsoft.Json;

namespace BinanceDex.Api.Models
{
    public class Times : ApiError
    {
        #region Properties

        /// <summary>
        ///     Event Time
        /// </summary>
        [JsonProperty("ap_time")]
        public DateTime ApTime { get; set; }

        /// <summary>
        ///     Block Height
        /// </summary>
        [JsonProperty("block_time")]
        public string BlockTime { get; set; }

        #endregion
    }
}