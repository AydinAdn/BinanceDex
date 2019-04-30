using Newtonsoft.Json;

namespace BinanceDex.WebSockets.Models
{
    public class WebSocketResponse<T>
    {

        [JsonProperty("stream")]
        public string Stream { get; set; }

        [JsonProperty("data")]
        public T Data { get; set; }
    }
}