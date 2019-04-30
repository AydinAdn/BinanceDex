using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using WebSocketSharp;

namespace BinanceDex.WebSockets
{
    public class WebSocketBase : IDisposable
    {
        private readonly string baseUrl;
        protected readonly WebSocket webSocket;
        private readonly bool keepConnected;
        private readonly CancellationTokenSource cts;

        protected WebSocketBase(string baseUrl, bool keepConnected) : this(baseUrl)
        {
            this.keepConnected = keepConnected;
            this.cts = new CancellationTokenSource();
        }

        protected WebSocketBase(string baseUrl)
        {
            this.baseUrl = baseUrl;
            this.webSocket = new WebSocket(baseUrl);
            this.webSocket.EmitOnPing = true;
        }

        private Task KeepAlive(CancellationToken token)
        {
            return Task.Run(async () =>
            {
                await Task.Delay(TimeSpan.FromMinutes(25), token);
                if (this.webSocket.IsAlive)
                {
                    this.Send(@"{ ""method"": ""keepAlive"" }");
                    this.KeepAlive(token);
                }
            }, token);
        }

        public void Connect()
        {
            if (this.webSocket.IsAlive) return;

            this.webSocket.Connect();
            if (this.keepConnected)
            {
                this.KeepAlive(this.cts.Token);
            }
        }

        protected void Send(string data)
        {
            this.webSocket.Send(data);
        }


        public void Dispose()
        {
            this.Send(@"{""method"": ""close""}");
            ((IDisposable) this.webSocket)?.Dispose();
            this.cts?.Dispose();
        }
    }


    public class SubscriptionOptions
    {

        [JsonProperty("method")]
        public string Method { get; set; }

        [JsonProperty("topic")]
        public string Topic { get; set; }

        [JsonProperty("symbols")]
        public List<string> Symbols { get; set; }
    }
}
