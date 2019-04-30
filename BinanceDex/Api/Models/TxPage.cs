using System.Collections.Generic;
using Newtonsoft.Json;

namespace BinanceDex.Api.Models
{
    public class Tx
    {
        [JsonProperty("txHash")]
        public string TxHash { get; set; }

        [JsonProperty("blockHeight")]
        public int BlockHeight { get; set; }

        [JsonProperty("txType")]
        public string TxType { get; set; }

        [JsonProperty("timeStamp")]
        public string TimeStamp { get; set; }

        [JsonProperty("fromAddr")]
        public string FromAddr { get; set; }

        [JsonProperty("toAddr")]
        public string ToAddr { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }

        [JsonProperty("txAsset")]
        public string TxAsset { get; set; }

        [JsonProperty("txFee")]
        public string TxFee { get; set; }

        [JsonProperty("txAge")]
        public int TxAge { get; set; }

        [JsonProperty("orderId")]
        public string OrderId { get; set; }

        [JsonProperty("code")]
        public int Code { get; set; }

        [JsonProperty("data")]
        public string Data { get; set; }

        [JsonProperty("confirmBlocks")]
        public int ConfirmBlocks { get; set; }

        [JsonProperty("memo")]
        public string Memo { get; set; }
    }

    public class TxPage
    {
        [JsonProperty("tx")]
        public IList<Tx> Tx { get; set; }

        [JsonProperty("total")]
        public int Total { get; set; }
    }
}
