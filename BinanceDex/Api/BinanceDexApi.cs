using BinanceDex.Api.Models;
using BinanceDex.Api.RequestOptions;
using BinanceDex.Utilities;
using BinanceDex.Utilities.Extensions;
using MessagePack;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using BinanceDex.RateLimit;

namespace BinanceDex.Api
{
    /// <inheritdoc />
    public class BinanceDexApi : IRateLimiter, IBinanceDexApi
    {
        private readonly IHttp http;
        private readonly string baseUrl;
        private bool useRateLimiter = true;
        private IRateLimiter rateLimiterImplementation;
        public string Hrp { get; }


        /// <summary>
        ///     Creates a the binance dex rest api.
        /// </summary>
        /// <param name="http">The http object to use</param>
        /// <param name="baseUrl">The base url of the api</param>
        /// <param name="hrp">The `hrp` or `human readable part` associated with the environment (tbnb for testing, bnb for live) </param>
        public BinanceDexApi(IHttp http, string baseUrl, string hrp) : this(http,baseUrl, hrp, new RateLimiter<IBinanceDexApi>())
        {
        }

        public BinanceDexApi(IHttp http, string baseUrl, string hrp, IRateLimiter rateLimiter)
        {

            Throw.IfNull(http, nameof(http));
            Throw.IfNullOrWhiteSpace(baseUrl, nameof(baseUrl));

            this.http = http;
            this.baseUrl = baseUrl;
            this.Hrp = hrp;
            this.rateLimiterImplementation = rateLimiter;
        }


        public async Task<Times> GetTimeAsync()
        {
            string path = "time";

            var limiter = this.GetRateLimiter();

            HttpResponse result  = await limiter.Try(() => this.http.GetAsync(this.baseUrl + path));

            return JsonConvert.DeserializeObject<Times>(result.Response);
        }

        public async Task<Node> GetNodeInfoAsync()
        {
            string path = "node-info";

            HttpResponse result = await this.http.GetAsync(this.baseUrl + path);

            return JsonConvert.DeserializeObject<Node>(result.Response);
        }

        public async Task<Validators> GetValidatorsAsync()
        {
            string path = "validators";

            HttpResponse result = await this.http.GetAsync(this.baseUrl + path);

            return JsonConvert.DeserializeObject<Validators>(result.Response);
        }

        public async Task<IEnumerable<Peer>> GetPeersAsync()
        {
            string path = "peers";

            HttpResponse result = await this.http.GetAsync(this.baseUrl + path);

            return JsonConvert.DeserializeObject<IList<Peer>>(result.Response);
        }

        public async Task<Account> GetAccountAsync(string address)
        {
            Throw.IfNullOrWhiteSpace(address, nameof(address));
            string path = "account/"+address;


            HttpResponse result = await this.http.GetAsync(this.baseUrl + path);


            return JsonConvert.DeserializeObject<Account>(result.Response);
        }

        public async Task<AccountSequence> GetAccountSequenceAsync(string address)
        {
            Throw.IfNullOrWhiteSpace(address, nameof(address));
            string path = $"account/{address}/sequence";

            HttpResponse result = await this.http.GetAsync(this.baseUrl + path);

            return JsonConvert.DeserializeObject<AccountSequence>(result.Response);
        }


        public async Task<Transaction> GetTransactionAsync(string transactionId)
        {
            Throw.IfNullOrWhiteSpace(transactionId, nameof(transactionId));
            string path = $"tx/{transactionId}";

            HttpResponse result = await this.http.GetAsync(this.baseUrl + path);

            return JsonConvert.DeserializeObject<Transaction>(result.Response);
        }

        public async Task<IEnumerable<Token>> GetTokensAsync(int limit = 500, int offset = 0)
        {
            if (limit <= 0) throw new ArgumentOutOfRangeException(nameof(limit), "Limit must be more than 0");
            if (offset < 0) throw new ArgumentOutOfRangeException(nameof(limit), "Offset must be 0 or more.");

            if (limit > 1000) limit = 1000;

            string path = $"tokens?limit={limit}&offset={offset}";

            HttpResponse result = await this.http.GetAsync(this.baseUrl + path);


            return JsonConvert.DeserializeObject<IList<Token>>(result.Response);
        }

        public async Task<MarketPairs> GetMarketPairsAsync(int limit = 500, int offset = 0)
        {
            if (limit <= 0) throw new ArgumentOutOfRangeException(nameof(limit), "Limit must be more than 0");
            if (offset < 0) throw new ArgumentOutOfRangeException(nameof(limit), "Offset must be 0 or more.");

            if (limit > 1000) limit = 1000;

            string path = "markets";

            HttpResponse result = await this.http.GetAsync(this.baseUrl + path);

            if (result.StatusCode == 200)
            {
                var markets =  JsonConvert.DeserializeObject<IList<Market>>(result.Response);
                return new MarketPairs{Markets = markets};
            }

            return JsonConvert.DeserializeObject<MarketPairs>(result.Response);
        }

        public async Task<IEnumerable<Fee>> GetFeesAsync()
        {
            string path = "fees";

            HttpResponse result = await this.http.GetAsync(this.baseUrl + path);

            return JsonConvert.DeserializeObject<IList<Fee>>(result.Response);
        }

        public async Task<MarketDepth> GetOrderBookAsync(string symbol, int? limit = null)
        {
            string path = $"depth?symbol={symbol}";
            if (limit.HasValue)
            {
                if (limit < 5) limit = 5;
                else if (limit < 10) limit = 10;
                else if (limit < 20) limit = 20;
                else if (limit < 50) limit = 50;
                else if (limit < 100) limit = 100;
                else if (limit < 500) limit = 500;
                else limit = 1000;
                path += $"?limit={limit}";
            }

            HttpResponse result = await this.http.GetAsync(this.baseUrl + path);

            return JsonConvert.DeserializeObject<MarketDepth>(result.Response, new JsonSerializerSettings{});
        }

        public async Task<IEnumerable<CandleStick>> GetCandleSticksAsync(GetCandleStickOptions candleStickOptions)
        {
            Throw.IfNullOrWhiteSpace(candleStickOptions.Symbol);
            Throw.IfNull(candleStickOptions.Interval, nameof(candleStickOptions.Interval));

            string path = "klines";
                
            if (candleStickOptions.Limit < 0) throw new ArgumentOutOfRangeException(nameof(candleStickOptions.Limit));
            if (candleStickOptions.Limit > 1000) candleStickOptions.Limit = 1000;

            path += candleStickOptions.ToString();

            HttpResponse result = await this.http.GetAsync(this.baseUrl + path);

            byte[] bytes = MessagePackSerializer.FromJson(result.Response);
            return  MessagePackSerializer.Deserialize<CandleStick[]>(bytes);
        }

        // TODO: Transform parameters
        public async Task<Orders> GetClosedOrdersAsync(string address, string symbol = null, int limit = 300, int offset = 0, long? startMs = null, long? endMs = null, OrderSide? orderSide = null, int? total = 0)
        {
            string path = $"orders/closed?address={address}";

            path += $"&limit={limit}";
            path += $"&offset={offset}";
            path += symbol   ?.Prepend("&symbol=");
            path += startMs  ?.ToString().Prepend("&start=");
            path += endMs    ?.ToString().Prepend("&end=");
            path += total    ?.ToString().Prepend("&total=");
            path += orderSide?.GetInt().ToString().Prepend("&side=");

            HttpResponse result = await this.http.GetAsync(this.baseUrl + path);

            return JsonConvert.DeserializeObject<Orders>(result.Response);
        }

        // TODO: Transform parameters
        public async Task<Orders> GetOpenOrdersAsync(string address, int limit = 500, int offset = 0, string symbol = null, int? total = null)
        {
            string path = $"orders/open?address={address}";
            path += $"&limit={limit}";
            path += $"&offset={offset}";
            path += symbol?.Prepend("&symbol=");
            path += total?.ToString().Prepend("&total=");

            HttpResponse result = await this.http.GetAsync(this.baseUrl + path);

            return JsonConvert.DeserializeObject<Orders>(result.Response);

        }

        public async Task<Order> GetOrderAsync(string orderId)
        {
            Throw.IfNullOrWhiteSpace(orderId, nameof(orderId));

            string path = $"orders/{orderId}";

            HttpResponse result = await this.http.GetAsync(path);

            return JsonConvert.DeserializeObject<Order>(result.Response);
        }

        public async Task<IEnumerable<Ticker>> GetTickerStatistics(string symbol = null)
        {
            string path = "ticker/24hr";
            path += symbol?.Prepend("?symbol=");

            HttpResponse result = await this.http.GetAsync(this.baseUrl + path);

            return JsonConvert.DeserializeObject<IList<Ticker>>(result.Response);
        }

        public async Task<TradePage> GetTradesAsync(GetTradeOptions options)
        {
            string path = "trades";
            path += options.ToString();

            HttpResponse result = await this.http.GetAsync(this.baseUrl + path);

            return JsonConvert.DeserializeObject<TradePage>(result.Response);
        }

        public async Task<BlockExchangeFeePage> GetBlockExchangeFeeAsync(GetBlockFeeOptions options)
        {
            string path = "block-exchange-fee";
            path += options.ToString();

            var result = await this.http.GetAsync(this.baseUrl + path);

            return JsonConvert.DeserializeObject<BlockExchangeFeePage>(result.Response);
        }

        public async Task<TxPage> GetTransactionsAsync(GetTxOptions options)
        {
            string path = "transactions";
            path += options.ToString();

            var result = await this.http.GetAsync(this.baseUrl + path);

            return JsonConvert.DeserializeObject<TxPage>(result.Response);

        }

        public Limiter GetRateLimiter([CallerMemberName]string endpoint = "")
        {
            return this.rateLimiterImplementation.GetRateLimiter(endpoint);
        }
    }
}