using Newtonsoft.Json;

namespace BinanceDex.Api.Models
{
    public class DexFeeField
    {
        #region Properties

        [JsonProperty("fee_name")]
        public string FeeName { get; set; }

        [JsonProperty("fee_value")]
        public int FeeValue { get; set; }

        #endregion
    }
}