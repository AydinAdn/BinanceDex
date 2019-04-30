using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using BinanceDex.Api;
using BinanceDex.Api.Models;
using BinanceDex.Api.RequestOptions;
using BinanceDex.Utilities;
using BinanceDex.Utilities.Extensions;
using BinanceDex.WebSockets;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NBitcoin;

namespace Binance.CandleStickStreamSample
{
    public class Program
    {
        private static long _height;
        private static object _locker;

        static async Task Main(string[] args)
        {
            await SingleStream();

            // TODO
            // await CombinedStream();
        }

        public static async Task SingleStream()
        {
            #region IoC, logging, configuration setup

            _height = 0;
            _locker = new object();

            IConfigurationRoot configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                                                             .AddJsonFile("appsettings.json", true, false)
                                                             .Build();

            ServiceProvider services = new ServiceCollection().AddLogging(builder => builder.SetMinimumLevel(LogLevel.Trace)
                                                                                            .AddFile(configuration.GetSection("Logging:File")))
                                                              .AddDexApi(configuration)
                                                              .AddDexWebSockets(configuration)
                                                              .AddAutoMapperConfiguration()
                                                              .BuildServiceProvider();
            #endregion

            #region Options

            // Choose the symbol you want to track, the limit of the cache and the interval you want to track.
            const string symbol = @"OWT-303_BNB";
            const int limit = 30;
            const CandleStickInterval interval = CandleStickInterval.Minutes_1;

            #endregion

            // Get Services
            IBinanceDexApi api = services.GetService<IBinanceDexApi>();
            IMapper objectMapper = services.GetService<IMapper>();
            ICandleStickWebSocket candleStickWebSocket = services.GetService<ICandleStickWebSocket>();

        
            // Get initial kline history from API and print to console
            IEnumerable<CandleStick> candles = await api.GetCandleSticksAsync(new GetCandleStickOptions { Symbol = symbol, Interval = interval, Limit = limit });
            Display(candles);


            // Storing the history in a sorted dictionary which we'll use as a cache when using the websockets
            Dictionary<long, CandleStick> csTemp = candles.ToDictionary(candleStick => candleStick.OpenTime, candleStick => candleStick);
            SortedDictionary<long, CandleStick> candleStickCache = new SortedDictionary<long, CandleStick>(csTemp, new ReverseComparer<long>());

           
            // Configuring the websocket
            candleStickWebSocket.OnDisconnect += (sender, eventArgs) => Console.WriteLine("Disconnected");
            candleStickWebSocket.OnCandleStickUpdate += (sender, update) =>
            {
                // Using a lock to avoid competing for the console window
                lock (_locker)
                {
                    // In case data arrives out of order, check event height is > previous height
                    if (update.EventHeight < _height) return;

                    _height = update.EventHeight;

                    // Update our cache
                    candleStickCache.AddOrReplace(update.CandleStick.OpenTime, objectMapper.Map<CandleStick>(update.CandleStick));

                    // Remove old candles as new ones arrive, maintaining the buffer specified in the `limit` const
                    if (candleStickCache.Count > limit)
                    {
                        candleStickCache.Skip(limit)
                                        .ToList()
                                        .ForEach(c => candleStickCache.Remove(c.Key));
                    }

                    // Print to console
                    Display(candleStickCache.Select(x => x.Value).Reverse());
                }

            };

            candleStickWebSocket.Connect();
            candleStickWebSocket.Subscribe(symbol, interval);
            await Console.In.ReadLineAsync();

            candleStickWebSocket.UnsubscribeAll();
            candleStickWebSocket.Close();
            candleStickWebSocket.Dispose();
        }

        // TODO: Demonstrate combined stream
        public async Task CombinedStream()
        {

        }

        public static void Display(IEnumerable<CandleStick> candles)
        {
            Console.SetCursorPosition(0, 1);
            int pad = 14;
            foreach (var candlestick in candles)
            {
                Console.Write($"[{candlestick.OpenTime.ToDateTime()}]  -  ");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write($"O: {candlestick.Open.ToString("0.00000000").PadLeft(pad)} | ");
                
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.Write($"H: {candlestick.High.ToString("0.00000000").PadLeft(pad)} | ");

                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.Write($"L: {candlestick.Low.ToString("0.00000000").PadLeft(pad)} | ");

                Console.ForegroundColor = ConsoleColor.Blue;
                Console.Write($"C: {candlestick.Close.ToString("0.00000000").PadLeft(pad)} | ");

                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine($"V: {candlestick.Volume.ToString("0.00").PadLeft(pad)}".PadRight(119));
                Console.ResetColor();
            }
            Console.WriteLine();
        }
    }

    public class ReverseComparer<T> : IComparer<T> where T : IComparable<T>
    {
        public int Compare(T x, T y)
        {
            return y.CompareTo(x);
        }
    }
}
