using Newtonsoft.Json;

namespace BinanceDex.Api.Models
{
    public class ValidatorInfo
    {
        #region Properties

        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("pub_key")]
        public byte[] PubKey { get; set; }

        [JsonProperty("voting_power")]
        public long VotingPower { get; set; }

        #endregion
    }
}