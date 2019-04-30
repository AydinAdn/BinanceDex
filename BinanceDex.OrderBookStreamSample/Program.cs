using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BinanceDex.Api;
using BinanceDex.Api.Models;
using BinanceDex.Utilities;
using BinanceDex.Utilities.Extensions;
using BinanceDex.WebSockets;

namespace BinanceDex.OrderBookStreamSample
{
    class Program
    {
        public static object locker= new object();
        static async Task Main(string[] args)
        {
            BinanceDexApi binanceDexApi = new BinanceDexApi(new Http(), "https://testnet-dex.binance.org/api/v1/", "tbnb");

            string symbol = "TCAN-014_BNB";
            MarketDepth marketDepth = await binanceDexApi.GetOrderBookAsync(symbol);

            OrderBook orderbook = new OrderBook(symbol, marketDepth.Height, marketDepth.Bids, marketDepth.Asks);

            Print(orderbook, 5);
            MarketDiffWebSocket marketDiff = new MarketDiffWebSocket("wss://testnet-dex.binance.org/api/ws", true);

            marketDiff.OnMarketDiffUpdate += (o, e) => 
            {
                orderbook.Modify(e.EventHeight, e.Bids, e.Asks);
                Print(orderbook, 5);
            };

            marketDiff.Connect();
            marketDiff.Subscribe(symbol);

            await Console.In.ReadLineAsync();

            marketDiff.UnsubscribeAll();
            marketDiff.Dispose();
        }

        private static void Print(OrderBook orderBook, int limit)
        {
            if (orderBook == null) throw new ArgumentNullException(nameof(orderBook));

            lock (locker)
            {
                Console.SetCursorPosition(0, 0);

                var asks = orderBook.Asks.Take(limit).Reverse();
                var bids = orderBook.Bids.Take(limit);

                Console.ForegroundColor = ConsoleColor.Red;

                foreach (var ask in asks)
                {
                    var bars = new StringBuilder("|");
                    if (ask.Quantity > 50)
                    {
                        bars.Append("---------------------  50 +  ---------------------");
                    }
                    else
                    {
                        var size = ask.Quantity;

                        while (size-- >= 1)
                            bars.Append("-");

                        while (bars.Length < 51)
                            bars.Append(" ");
                    }

                    Console.WriteLine($" {ask.Price.ToString("0.00000000").PadLeft(14)} {ask.Quantity.ToString("0.00000000").PadLeft(15)}  {bars}");
                }

                Console.ResetColor();

                Console.WriteLine();
                Console.WriteLine($"  {orderBook.MidMarketPrice().ToString("0.0000000000").PadLeft(16)} {orderBook.Spread().ToString("0.00000000").PadLeft(17)} Spread");
                Console.WriteLine();

                Console.ForegroundColor = ConsoleColor.Green;

                foreach (var bid in bids)
                {
                    var bars = new StringBuilder("|");
                    if (bid.Quantity > 50)
                    {
                        bars.Append("---------------------  50 +  ---------------------");
                    }
                    else
                    {
                        var size = bid.Quantity;

                        while (size-- >= 1)
                            bars.Append("-");

                        while (bars.Length < 51)
                            bars.Append(" ");
                    }

                    Console.WriteLine($" {bid.Price.ToString("0.00000000").PadLeft(14)} {bid.Quantity.ToString("0.00000000").PadLeft(15)}  {bars}");
                }

                Console.ResetColor();
            }
        }
    }
}
