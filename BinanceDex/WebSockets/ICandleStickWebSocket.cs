using System;
using System.Collections.Generic;
using BinanceDex.Api.Models;
using BinanceDex.WebSockets.Models;

namespace BinanceDex.WebSockets
{
    public interface ICandleStickWebSocket : IDisposable
    {
        void Connect();
        void Subscribe(List<string> symbols, CandleStickInterval interval);
        void Subscribe(string symbol, CandleStickInterval interval);
        void Unsubscribe(string symbol, CandleStickInterval interval);
        void Unsubscribe(List<string> symbols, CandleStickInterval interval);
        void UnsubscribeAll();
        void Close();
        event EventHandler OnConnect;
        event EventHandler OnDisconnect;
        event EventHandler<KLineUpdate> OnCandleStickUpdate;
    }
}