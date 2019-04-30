using System.Collections.Generic;
using Newtonsoft.Json;

namespace BinanceDex.Api.Models
{
    public class Account : ApiError
    {
        #region Properties

        [JsonProperty("account_number")]
        public long AccountNumber { get; set; }

        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("balances")]
        public IList<Balance> Balances { get; set; }

        [JsonProperty("public_key")]
        public byte[] PublicKey { get; set; }

        [JsonProperty("sequence")]
        public long Sequence { get; set; }

        #endregion
    }
}