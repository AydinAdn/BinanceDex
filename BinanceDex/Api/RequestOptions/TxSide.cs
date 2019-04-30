using BinanceDex.Api.Models;

namespace BinanceDex.Api.RequestOptions
{
    public enum TxSide
    {
        [Descriptor("RECEIVE")]
        Receive,
        [Descriptor("SEND")]
        Send
    }
}