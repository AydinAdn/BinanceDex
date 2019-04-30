using Newtonsoft.Json;

namespace BinanceDex.Api.Models
{
    public class AccountSequence : ApiError
    {
        #region Properties

        [JsonProperty("sequence")]
        public long Sequence { get; set; }

        #endregion
    }
}