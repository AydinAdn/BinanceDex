using System.Collections.Generic;
using Newtonsoft.Json;

namespace BinanceDex.Api.Models
{
    public class Orders : ApiError
    {
        #region Properties

        [JsonProperty("order")]
        public IList<Order> Order { get; set; }

        [JsonProperty("total")]
        public int Total { get; set; }

        #endregion
    }
}