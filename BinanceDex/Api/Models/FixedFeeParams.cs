using Newtonsoft.Json;

namespace BinanceDex.Api.Models
{
    public class FixedFeeParams
    {
        #region Properties

        [JsonProperty("fee")]
        public long Fee { get; set; }

        [JsonProperty("fee_for")]
        public FeeFor FeeFor { get; set; }

        [JsonProperty("msg_type")]
        public string MsgType { get; set; }

        #endregion
    }
}