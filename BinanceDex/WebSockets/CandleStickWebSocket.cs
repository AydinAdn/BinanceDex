using System;
using System.Collections.Generic;
using System.Linq;
using BinanceDex.Api.Models;
using BinanceDex.Utilities.Extensions;
using BinanceDex.WebSockets.Models;
using Newtonsoft.Json;
using WebSocketSharp;

namespace BinanceDex.WebSockets
{
    public class CandleStickWebSocket : WebSocketBase, ICandleStickWebSocket
    {
        private readonly Dictionary<CandleStickInterval, List<string>> symbolsSubscribedTo;

        public CandleStickWebSocket(string baseUrl, bool keepConnected) : base(baseUrl, keepConnected)
        {
            this.symbolsSubscribedTo = new Dictionary<CandleStickInterval, List<string>>();
            this.webSocket.OnMessage += this.WebSocket_OnMessage;
            this.webSocket.OnOpen += (o, e) => this.OnConnect?.Invoke(this, null);
            this.webSocket.OnClose += (o, e) => this.OnDisconnect?.Invoke(this, null);
        }

        public CandleStickWebSocket(string baseUrl) : this(baseUrl, false)
        {
        }

        public event EventHandler OnConnect;
        public event EventHandler OnDisconnect;
        public event EventHandler<KLineUpdate> OnCandleStickUpdate;

        private void WebSocket_OnMessage(object sender, MessageEventArgs e)
        {
            WebSocketResponse<KLineUpdate> result = JsonConvert.DeserializeObject<WebSocketResponse<KLineUpdate>>(e.Data);
                              
            this.OnCandleStickUpdate?.Invoke(this, result.Data);
        }

        public void Subscribe(List<string> symbols, CandleStickInterval interval)
        {
            SubscriptionOptions subOptions = new SubscriptionOptions
            {
                Method = "subscribe",
                Topic = "kline_" + interval.GetAttribute<Descriptor>().Identifier,
                Symbols = symbols
            };

            if (!this.symbolsSubscribedTo.ContainsKey(interval))
            {
                this.symbolsSubscribedTo.Add(interval, new List<string>());
                
            }

            foreach (string symbol in subOptions.Symbols)
            {
                if (!this.symbolsSubscribedTo[interval].Contains(symbol))
                {
                    this.symbolsSubscribedTo[interval].Add(symbol);
                }
                else
                {
                    subOptions.Symbols.Remove(symbol);
                }
            }

            this.webSocket.Send(JsonConvert.SerializeObject(subOptions));
        }

        public void Subscribe(string symbol, CandleStickInterval interval)
        {
            this.Subscribe(new List<string>{symbol}, interval);
        }

        public void Unsubscribe(string symbol, CandleStickInterval interval)
        {
            this.Unsubscribe(new List<string>(){symbol}, interval);
        }

        public void Unsubscribe(List<string> symbols, CandleStickInterval interval)
        {
            SubscriptionOptions subOptions = new SubscriptionOptions
            {
                Method = "unsubscribe",
                Topic = "kline_" + interval.GetAttribute<Descriptor>().Identifier,
                Symbols = symbols
            };

            this.symbolsSubscribedTo[interval].RemoveAll(x => subOptions.Symbols.Contains(x));

            this.webSocket.Send(JsonConvert.SerializeObject(subOptions));
        }

        public void UnsubscribeAll()
        {
            foreach (var pair in this.symbolsSubscribedTo)
            {
                this.Unsubscribe(pair.Value, pair.Key);
            }

            this.symbolsSubscribedTo.Clear();
        }

        public void Close()
        {
            this.UnsubscribeAll();
            this.webSocket.Close();
        }
    }



    public class MarketDiffWebSocket : WebSocketBase
    {
        private readonly List<string> symbolsSubscribedTo;

        public MarketDiffWebSocket(string baseUrl, bool keepConnected) : base(baseUrl, keepConnected)
        {
            this.symbolsSubscribedTo = new List<string>();
            this.webSocket.OnMessage += this.WebSocket_OnMessage;
            this.webSocket.OnOpen += (o, e) => this.OnConnect?.Invoke(this, null);
            this.webSocket.OnClose += (o, e) => this.OnDisconnect?.Invoke(this, null);
        }

        public MarketDiffWebSocket(string baseUrl) : this(baseUrl, false)
        {
        }

        public event EventHandler OnConnect;
        public event EventHandler OnDisconnect;
        public event EventHandler<MarketDiffUpdate> OnMarketDiffUpdate;

        private void WebSocket_OnMessage(object sender, MessageEventArgs e)
        {
            if (e.Data.IsNullOrWhiteSpace())
            {
                return;
            }
            WebSocketResponse<MarketDiffUpdate> result = JsonConvert.DeserializeObject<WebSocketResponse<MarketDiffUpdate>>(e.Data);

            this.OnMarketDiffUpdate?.Invoke(this, result.Data);
        }

        public void Subscribe(List<string> symbols)
        {
            string[] toAdd = symbols.Except(this.symbolsSubscribedTo).ToArray();

            if (!toAdd.Any()) return;

            foreach (string symbol in toAdd)
            {
                this.symbolsSubscribedTo.Add(symbol);
            }

            SubscriptionOptions subOptions = new SubscriptionOptions
            {
                Method = "subscribe",
                Topic = "marketDiff",
                Symbols = symbols
            };

            this.webSocket.Send(JsonConvert.SerializeObject(subOptions));
        }

        public void Subscribe(string symbol)
        {
            this.Subscribe(new List<string> { symbol });
        }

        public void Unsubscribe(string symbol)
        {
            this.Unsubscribe(new List<string>() { symbol });
        }

        public void Unsubscribe(List<string> symbols)
        {
            SubscriptionOptions subOptions = new SubscriptionOptions
            {
                Method = "unsubscribe",
                Topic = "marketDiff",
                Symbols = symbols
            };

            this.symbolsSubscribedTo.RemoveAll(symbols.Contains);

            this.webSocket.Send(JsonConvert.SerializeObject(subOptions));
        }

        public void UnsubscribeAll()
        {
            foreach (var symbol in this.symbolsSubscribedTo)
            {
                this.Unsubscribe(symbol);
            }

            this.symbolsSubscribedTo.Clear();
        }

        public void Close()
        {
            this.UnsubscribeAll();
            this.webSocket.Close();
        }
    }

    public class TradesWebSocket : WebSocketBase
    {
        private readonly List<string> symbolsSubscribedTo;

        public TradesWebSocket(string baseUrl, bool keepConnected) : base(baseUrl, keepConnected)
        {
            this.symbolsSubscribedTo = new List<string>();
            this.webSocket.OnMessage += this.WebSocket_OnMessage;
            this.webSocket.OnOpen += (o, e) => this.OnConnect?.Invoke(this, null);
            this.webSocket.OnClose += (o, e) => this.OnDisconnect?.Invoke(this, null);
        }

        public TradesWebSocket(string baseUrl) : this(baseUrl, false)
        {
        }

        public event EventHandler OnConnect;
        public event EventHandler OnDisconnect;
        public event EventHandler<TradeUpdate> OnMarketDiffUpdate;

        private void WebSocket_OnMessage(object sender, MessageEventArgs e)
        {
            WebSocketResponse<TradeUpdate> result = JsonConvert.DeserializeObject<WebSocketResponse<TradeUpdate>>(e.Data);

            this.OnMarketDiffUpdate?.Invoke(this, result.Data);
        }

        public void Subscribe(List<string> symbols)
        {
            string[] toAdd = symbols.Except(this.symbolsSubscribedTo).ToArray();

            if (!toAdd.Any()) return;

            foreach (string symbol in toAdd)
            {
                this.symbolsSubscribedTo.Add(symbol);
            }

            SubscriptionOptions subOptions = new SubscriptionOptions
            {
                Method = "subscribe",
                Topic = "trades",
                Symbols = symbols
            };

            this.webSocket.Send(JsonConvert.SerializeObject(subOptions));
        }

        public void Subscribe(string symbol)
        {
            this.Subscribe(new List<string> { symbol });
        }

        public void Unsubscribe(string symbol)
        {
            this.Unsubscribe(new List<string>() { symbol });
        }

        public void Unsubscribe(List<string> symbols)
        {
            SubscriptionOptions subOptions = new SubscriptionOptions
            {
                Method = "unsubscribe",
                Topic = "trades",
                Symbols = symbols
            };

            this.symbolsSubscribedTo.RemoveAll(symbols.Contains);

            this.webSocket.Send(JsonConvert.SerializeObject(subOptions));
        }

        public void UnsubscribeAll()
        {
            foreach (var symbol in this.symbolsSubscribedTo)
            {
                this.Unsubscribe(symbol);
            }

            this.symbolsSubscribedTo.Clear();
        }

        public void Close()
        {
            this.UnsubscribeAll();
            this.webSocket.Close();
        }
    }
}