using System.Collections.Generic;
using Newtonsoft.Json;

namespace BinanceDex.Api.Models
{
    public class Validators : ApiError
    {
        #region Properties

        [JsonProperty("block_height")]
        public long BlockHeight { get; set; }

        [JsonProperty("validators")]
        public IList<ValidatorInfo> ValidatorCollection { get; set; }

        #endregion
    }
}