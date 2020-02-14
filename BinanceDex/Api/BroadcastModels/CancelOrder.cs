using ProtoBuf;

namespace BinanceDex.Api.BroadcastModels
{
    public class CancelOrder
    {
        [ProtoMember(1)]
        public string Symbol { get; set; }

        [ProtoMember(2)]
        public string RefId { get; set; }
    }
}
