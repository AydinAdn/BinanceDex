using Newtonsoft.Json;

namespace BinanceDex.Api.Models
{
    public class NodeInfoOther
    {
        #region Properties

        [JsonProperty("amino_version")]
        public string AminoVersion { get; set; }

        [JsonProperty("consensus_version")]
        public string ConsensusVersion { get; set; }

        [JsonProperty("p2p_version")]
        public string P2PVersion { get; set; }

        [JsonProperty("rpc_address")]
        public string RpcAddress { get; set; }

        [JsonProperty("rpc_version")]
        public string RpcVersion { get; set; }

        [JsonProperty("tx_index")]
        public string TxIndex { get; set; }

        #endregion
    }
}