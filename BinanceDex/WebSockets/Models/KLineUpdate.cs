using System;
using Newtonsoft.Json;

namespace BinanceDex.WebSockets.Models
{
    public class KLineUpdate  : EventArgs
    {

        [JsonProperty("e")]
        public string EventType { get; set; }

        [JsonProperty("E")]
        public long EventHeight { get; set; }

        [JsonProperty("s")]
        public string Symbol { get; set; }

        [JsonProperty("k")]
        public KLine CandleStick { get; set; }
    }
}