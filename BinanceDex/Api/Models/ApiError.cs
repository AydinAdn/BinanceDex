using Newtonsoft.Json;

namespace BinanceDex.Api.Models
{
    public class ApiError
    {
        #region Properties

        [JsonProperty("code")]
        public int Code { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        #endregion
    }
}