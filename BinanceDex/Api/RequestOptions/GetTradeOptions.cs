using System;
using System.Text;
using System.Threading.Tasks;

namespace BinanceDex.Api.RequestOptions
{
    public class GetTradeOptions : OptionsBase
    {
        public string Address { get; set; }
        public string BuyOrderId { get; set; }
        public long? End { get; set; }
        public long? Height { get; set; }
        public int? Limit { get; set; }
        public int? Offset { get; set; }
        public string QuoteAsset { get; set; }
        public string SellerOrderId { get; set; }
        public int? Side { get; set; }
        public long? Start { get; set; }
        public string Symbol { get; set; }
        public int? Total { get; set; }
    }
}
