using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BinanceDex.Api.Models;
using BinanceDex.Api.RequestOptions;
using BinanceDex.RateLimit;
using NBitcoin;
using Transaction = BinanceDex.Api.Models.Transaction;

namespace BinanceDex.Api
{
    public interface IBinanceDexApi
    {
        string Hrp { get; }

        /// <summary>
        ///     Gets the latest block time and the current time.
        /// </summary>
        [RateLimiter(Rate = 1, PerTimeInSeconds = 1)]
        Task<Times> GetTimeAsync();

        /// <summary>
        ///     Gets runtime information about the node.
        /// </summary>
        [RateLimiter(Rate = 1, PerTimeInSeconds = 1)]
        Task<Node> GetNodeInfoAsync();

        /// <summary>
        ///     Gets the list of validators used in consensus
        /// </summary>
        [RateLimiter(Rate = 10, PerTimeInSeconds = 1)]
        Task<Validators> GetValidatorsAsync();

        /// <summary>
        ///     Gets the list of network peers
        /// </summary>
        [RateLimiter(Rate = 1, PerTimeInSeconds = 1)]
        Task<IEnumerable<Peer>> GetPeersAsync();


        /// <summary>
        ///     Gets account metadata
        /// </summary>
        /// <param name="address">The address of the account</param>
        [RateLimiter(Rate = 5, PerTimeInSeconds = 1)]
        Task<Account> GetAccountAsync(string address);


        /// <summary>
        ///     Gets the account sequence for an address
        /// </summary>
        /// <param name="address">The address of the account</param>
        [RateLimiter(Rate = 5, PerTimeInSeconds = 1)]
        Task<AccountSequence> GetAccountSequenceAsync(string address);

        /// <summary>
        ///     Gets the transaction metadata by transaction ID.
        /// </summary>
        /// <param name="transactionId">The transaction ID / hash </param>
        [RateLimiter(Rate = 10, PerTimeInSeconds = 1)]
        Task<Transaction> GetTransactionAsync(string transactionId);

        /// <summary>
        ///     Gets the list of tokens that have been issued.
        /// </summary>
        /// <param name="limit">Limit of results to return. Default is 500. Capped at 1000 </param>
        /// <param name="offset">Number of items to offset results by.</param>
        [RateLimiter(Rate = 1, PerTimeInSeconds = 1)]
        Task<IEnumerable<Token>> GetTokensAsync(int limit = 500, int offset = 0);


        /// <summary>
        ///     Gets the list of market pairs that have been listed
        /// </summary>
        /// <param name="limit">Limit of results to return. Default is 500. Capped at 1000 </param>
        /// <param name="offset">Number of items to offset results by.</param>
        [RateLimiter(Rate = 1, PerTimeInSeconds = 1)]
        Task<MarketPairs> GetMarketPairsAsync(int limit = 500, int offset = 0);


        /// <summary>
        ///     Gets the list of fees associated with trading on the binance chain
        /// </summary>
        [RateLimiter(Rate = 1, PerTimeInSeconds = 1)]
        Task<IEnumerable<Fee>> GetFeesAsync();


        /// <summary>
        ///     Returns an order book for the specified symbol
        /// </summary>
        /// <param name="symbol">The symbol to lookup</param>
        /// <param name="limit">The limit of results</param>
        [RateLimiter(Rate = 10, PerTimeInSeconds = 1)]
        Task<MarketDepth> GetOrderBookAsync(string symbol, int? limit = null);

        /// <summary>
        ///      Gets candlestick/kline bars for a symbol. Bars are uniquely identified by their open time.
        /// </summary>
        /// <param name="symbol">Symbol</param>
        /// <param name="interval">Interval</param>
        /// <param name="limit">Result limit</param>
        /// <param name="startTime">Start time, meaning show results starting from this date/time</param>
        /// <param name="endTime">End time, meaning show results ending on this date/time</param>
        [RateLimiter(Rate = 10, PerTimeInSeconds = 1)]
        Task<IEnumerable<CandleStick>> GetCandleSticksAsync(GetCandleStickOptions candleStickOptions);

        /// <summary>
        ///      Gets closed (filled and cancelled) orders for a given address.   
        /// </summary>
        /// <param name="address">The wallet address</param>
        /// <param name="symbol">Optionally filter to a particular symbol</param>
        /// <param name="limit">Limit result. Default is 300. Max 1000.</param>
        /// <param name="offset">Offset results by</param>
        /// <param name="startMs">Start time</param>
        /// <param name="endMs">End time</param>
        /// <param name="orderSide">Order side</param>
        /// <param name="total">Total number required, 0 for not required and 1 for required; default not required, return total=-1 in response</param>
        [RateLimiter(Rate = 5, PerTimeInSeconds = 1)]
        Task<Orders> GetClosedOrdersAsync(string address, string symbol = null, int limit = 300, int offset = 0, long? startMs = null, long? endMs = null, OrderSide? orderSide = null, int? total = 0);

        /// <summary>
        ///     Get open orders async    
        /// </summary>
        /// <param name="address">The wallet address</param>
        /// <param name="limit">Limit result. Default is 300. Max 1000.</param>
        /// <param name="offset">Offset results by</param>
        /// <param name="symbol">Optionally filter to a particular symbol</param>
        /// <param name="total">Total number required, 0 for not required and 1 for required; default not required, return total=-1 in response</param>
        [RateLimiter(Rate = 5, PerTimeInSeconds = 1)]
        Task<Orders> GetOpenOrdersAsync(string address, int limit = 500, int offset =0, string symbol = null, int? total = null);


        /// <summary>
        ///      Gets metadata for an individual order by its ID.    
        /// </summary>
        /// <param name="orderId">The order ID</param>
        [RateLimiter(Rate = 5, PerTimeInSeconds = 1)]
        Task<Order> GetOrderAsync(string orderId);

        /// <summary>
        ///     Gets 24 hour price change statistics for all symbols or a specific market pair symbol if one is specified. 
        /// </summary>
        /// <param name="symbol">The symbol to query</param>
        [RateLimiter(Rate = 5, PerTimeInSeconds = 1)]
        Task<IEnumerable<Ticker>> GetTickerStatistics(string symbol = null);


        [RateLimiter(Rate = 5, PerTimeInSeconds = 1)]
        Task<TradePage> GetTradesAsync(GetTradeOptions options);


        [RateLimiter(Rate = 5, PerTimeInSeconds = 1)]
        Task<BlockExchangeFeePage> GetBlockExchangeFeeAsync(GetBlockFeeOptions options);

        [RateLimiter(Rate = 60, PerTimeInSeconds = 60)]
        Task<TxPage> GetTransactionsAsync(GetTxOptions options);



        /*
         * API Methods left to implement
         * TODO: Broadcast
         * TODO: Transactions/
         *
         * Models
         * TODO: TxPage
         * TODO: ExchangeRate
         *      
         */
    }
}