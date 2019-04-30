using System.Collections.Generic;
using Newtonsoft.Json;

namespace BinanceDex.Api.Models
{
    public class Fee : ApiError
    {
        #region Properties

        [JsonProperty("dex_fee_fields")]
        public IList<DexFeeField> DexFeeFields { get; set; }

        [JsonProperty("fee_for")]
        public FeeFor FeeFor { get; set; }

        [JsonProperty("fee")]
        public long FeeValue { get; set; }

        [JsonProperty("fixed_fee_params")]
        public FixedFeeParams FixedFeeParams { get; set; }

        [JsonProperty("lower_limit_as_multi")]
        public int LowerLimitAsMulti { get; set; }

        [JsonProperty("msg_type")]
        public string MsgType { get; set; }

        [JsonProperty("multi_transfer_fee")]
        public int MultiTransferFee { get; set; }

        #endregion
    }
}