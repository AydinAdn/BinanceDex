using Newtonsoft.Json;

namespace BinanceDex.Api.Models
{
    public class Token : ApiError
    {
        #region Properties

        [JsonProperty("mintable")]
        public bool Mintable { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("original_symbol")]
        public string OriginalSymbol { get; set; }

        [JsonProperty("owner")]
        public string Owner { get; set; }

        [JsonProperty("symbol")]
        public string Symbol { get; set; }

        [JsonProperty("total_supply")]
        public string TotalSupply { get; set; }

        #endregion
    }
}