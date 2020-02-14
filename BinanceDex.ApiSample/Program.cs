using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using BinanceDex.Api;
using BinanceDex.Api.Models;
using BinanceDex.Api.RequestOptions;
using BinanceDex.Utilities;
using BinanceDex.Utilities.Extensions;
using ConsoleDump;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace BinanceDex.ApiSample
{
    static class Program
    {
        static async Task Main()
        {
            IConfigurationRoot configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                                                                         .AddJsonFile("appsettings.json", true, false)
                                                                         .Build();

            ServiceProvider services = new ServiceCollection().AddLogging(builder => builder.SetMinimumLevel(LogLevel.Trace)
                                                                                            .AddFile(configuration.GetSection("Logging:File")))
                                                              .AddDexApi(configuration)
                                                              .BuildServiceProvider();


            IBinanceDexApi api = services.GetService<IBinanceDexApi>();

            const string accountId = @"tbnb1zllq4cyrztmpxt7h4k92f8gpqkgmzwqpt82rek";
            const string txHash    = @"DBE9FA055E63D4B160640CD082F4DFFE8D7617467584F4F041CAC67D6CB27C44";
            const string symbol    = "XRP.B-585_BNB";
            const string orderId   = @"17FE0AE08312F6132FD7AD8AA49D010591B13801-1";

            // Get Time
            {
                Times timeResult = await api.GetTimeAsync();
                timeResult.Dump("Get Time"); // .Dump is just a helper method to print to console
            }


            // Get Node Info
            {
                Node node = await api.GetNodeInfoAsync();
                node.Dump("Get Node Info");
            }

            // Get Validators
            {
                Validators validators = await api.GetValidatorsAsync();
                validators.Dump("Get Validators");
            }

            // Get Peers
            {
                IEnumerable<Peer> peers = await api.GetPeersAsync();
                peers.Dump("Get Peers");
            }

            // Get Account
            {
                Account account = await api.GetAccountAsync(accountId);
                account.Dump("Get Account");
            }

            // Get Account Sequence
            {
                AccountSequence sequence = await api.GetAccountSequenceAsync(accountId);
                sequence.Dump("Get Account Sequence");
            }

            // Get Tx
            {
                Transaction transaction = await api.GetTransactionAsync(txHash);
                transaction.Dump("Get Tx");
            }

            // Get Tokens using default settings
            {
                IEnumerable<Token> tokens = await api.GetTokensAsync();
                tokens.Dump("Get Tokens");
            }

            // Get Markets
            {
                MarketPairs marketPairs = await api.GetMarketPairsAsync();
                marketPairs.Markets.Dump("Get Markets");
            }


            // Get Fees
            {
                IEnumerable<Fee> fees = await api.GetFeesAsync();
                fees.Dump("Get Fees");
            }


            // Get Order Book
            {
                MarketDepth orderBook = await api.GetOrderBookAsync(symbol);
                orderBook.Dump("Get Order Book");
            }


            // Get CandleSticks
            {
                IEnumerable<CandleStick> candleSticks = await api.GetCandleSticksAsync(new GetCandleStickOptions{Symbol = symbol, Interval =  CandleStickInterval.Minutes_15});
                candleSticks.Dump("Get CandleSticks");
            }

            // Get Closed Orders
            {
                Orders getClosedOrders = await api.GetClosedOrdersAsync(accountId, limit: 5);
                getClosedOrders.Dump("Get Closed Orders");
                getClosedOrders.Order.Dump();
            }
        }
    }
}
