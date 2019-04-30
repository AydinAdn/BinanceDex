using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BinanceDex.Api.Models;
using BinanceDex.Utilities;

namespace BinanceDex
{
    public sealed class OrderBook : ICloneable, IEquatable<OrderBook>
    {
        #region Public Properties

        /// <summary>
        /// Get the symbol.
        /// </summary>
        public string Symbol { get; }

        /// <summary>
        /// Get the last update ID.
        /// </summary>
        public long LastUpdateId { get; private set; }

        /// <summary>
        /// Get the order book top (best ask and bid) or null
        /// if either the bid or ask is not available.
        /// </summary>
        public OrderBookTop Top { get; private set; }

        /// <summary>
        /// Get the buyer bids in order of decreasing price.
        /// </summary>
        public IEnumerable<OrderBookPriceLevel> Bids { get; private set; }

        /// <summary>
        /// Get the seller asks in order of increasing price.
        /// </summary>
        public IEnumerable<OrderBookPriceLevel> Asks { get; private set; }

        #endregion Public Properties

        #region Private Fields

        private readonly SortedDictionary<decimal, OrderBookPriceLevel> _bids;
        private readonly SortedDictionary<decimal, OrderBookPriceLevel> _asks;

        #endregion Private Fields

        #region Constructors

        public OrderBook(string symbol, long lastUpdateId, IEnumerable<OrderBookPriceLevel> bids, IEnumerable<OrderBookPriceLevel> asks) : this(symbol, lastUpdateId, bids.Select(x=> new ValueTuple<decimal, decimal>(x.Price,x.Quantity)), asks.Select(x => new ValueTuple<decimal, decimal>(x.Price, x.Quantity)))
        {
        }

        /// <summary>
        /// Construct an order book.
        /// </summary>
        /// <param name="symbol">The symbol.</param>
        /// <param name="lastUpdateId">The last updated ID.</param>
        /// <param name="bids">The bids (price and aggregate quantity) in any sequence.</param>
        /// <param name="asks">The asks (price  and aggregate quantity) in any sequence.</param>
        public OrderBook(string symbol, long lastUpdateId, IEnumerable<(decimal, decimal)> bids, IEnumerable<(decimal, decimal)> asks)
        {
            Throw.IfNullOrWhiteSpace(symbol, nameof(symbol));
            Throw.IfNull(bids, nameof(bids));
            Throw.IfNull(asks, nameof(asks));

            if (lastUpdateId <= 0)
                throw new ArgumentException($"{nameof(OrderBook)} last update ID must be greater than 0.", nameof(lastUpdateId));

            this.Symbol = symbol;
            this.LastUpdateId = lastUpdateId;

            this._bids = new SortedDictionary<decimal, OrderBookPriceLevel>(new ReverseComparer<decimal>());
            this._asks = new SortedDictionary<decimal, OrderBookPriceLevel>();

            foreach (var bid in bids)
            {
                this._bids.Add(bid.Item1, new OrderBookPriceLevel(bid.Item1, bid.Item2));
            }

            foreach (var ask in asks)
            {
                this._asks.Add(ask.Item1, new OrderBookPriceLevel(ask.Item1, ask.Item2));
            }

            this.Bids = this._bids.Values.ToArray();
            this.Asks = this._asks.Values.ToArray();

            this.Top = this.Bids.Any() && this.Asks.Any() ? new OrderBookTop(this) : null;
        }

        #endregion Constructors

        #region Public Methods

        /// <summary>
        /// Get the aggregate quantity at a price level.
        /// </summary>
        /// <param name="price">The price level.</param>
        /// <returns>The quantity at price (0 if no entry at price).</returns>
        public decimal Quantity(decimal price)
        {
            return this._bids.ContainsKey(price) ? this._bids[price].Quantity
                : this._asks.ContainsKey(price) ? this._asks[price].Quantity : 0;
        }

        /// <summary>
        /// Get the sum quantity of bids at and above the price or
        /// the sum quantity of asks at and below the price.
        /// </summary>
        /// <param name="price">The price level (inclusive).</param>
        /// <returns>The order book depth up to price.</returns>
        public decimal Depth(decimal price)
        {
            return this._bids.TakeWhile(_ => _.Key >= price).Sum(_ => _.Value.Quantity)
                + this._asks.TakeWhile(_ => _.Key <= price).Sum(_ => _.Value.Quantity);
        }

        /// <summary>
        /// Get the sum volume (price * quantity) of bids at and above the
        /// price or the sum volume of asks at and below the price.
        /// </summary>
        /// <param name="price">The price level (inclusive).</param>
        /// <returns>The order book volume up to price.</returns>
        public decimal Volume(decimal price)
        {
            return this._bids.TakeWhile(_ => _.Key >= price).Sum(_ => _.Value.Price * _.Value.Quantity)
                + this._asks.TakeWhile(_ => _.Key <= price).Sum(_ => _.Value.Price * _.Value.Quantity);
        }

        #endregion Public Methods

        #region Internal Methods

        public void Modify(long lastUpdateId, IEnumerable<OrderBookPriceLevel> bids, IEnumerable<OrderBookPriceLevel> asks)
        {
            this.Modify(lastUpdateId, bids.Select(x=> new ValueTuple<decimal, decimal>(x.Price, x.Quantity)), asks.Select(x=> new ValueTuple<decimal, decimal>(x.Price, x.Quantity)));
        }

        /// <summary>
        /// Modify the order book.
        /// </summary>
        /// <param name="lastUpdateId"></param>
        /// <param name="bids"></param>
        /// <param name="asks"></param>
        internal void Modify(long lastUpdateId, IEnumerable<(decimal, decimal)> bids, IEnumerable<(decimal, decimal)> asks)
        {
            if (lastUpdateId <= this.LastUpdateId)
                throw new ArgumentException($"{nameof(OrderBook)}.{nameof(this.Modify)}: new ID must be greater than previous ID.");

            // Update order book bids.
            foreach (var bid in bids)
            {
                // If quantity is > 0, then set the quantity.
                if (bid.Item2 > 0)
                {
                    if (this._bids.ContainsKey(bid.Item1))
                        this._bids[bid.Item1].Quantity = bid.Item2;
                    else
                        this._bids[bid.Item1] = new OrderBookPriceLevel(bid.Item1, bid.Item2);
                }
                else // otherwise, remove the price level.
                    this._bids.Remove(bid.Item1);
            }

            // Update order book asks.
            foreach (var ask in asks)
            {
                // If quantity is > 0, then set the quantity.
                if (ask.Item2 > 0)
                {
                    if (this._asks.ContainsKey(ask.Item1))
                        this._asks[ask.Item1].Quantity = ask.Item2;
                    else
                        this._asks[ask.Item1] = new OrderBookPriceLevel(ask.Item1, ask.Item2);
                }
                else // otherwise, remove the price level.
                    this._asks.Remove(ask.Item1);
            }

            this.Bids = this._bids.Values.ToArray();
            this.Asks = this._asks.Values.ToArray();

            this.Top = this.Bids.Any() && this.Asks.Any() ? new OrderBookTop(this) : null;

            // Set the order book last update ID.
            this.LastUpdateId = lastUpdateId;
        }

        #endregion Internal Methods

        #region ICloneable

        /// <summary>
        /// Get a duplicate order book (deep copy).
        /// </summary>
        /// <returns></returns>
        public OrderBook Clone()
        {
            return new OrderBook(this.Symbol, this.LastUpdateId, this._bids.Select(_ => (_.Key, _.Value.Quantity)), this._asks.Select(_ => (_.Key, _.Value.Quantity)));
        }

        /// <summary>
        /// Get a duplicate order book (deep copy).
        /// </summary>
        /// <returns></returns>
        public OrderBook Clone(int limit)
        {
            if (limit <= 0) throw new ArgumentOutOfRangeException(nameof(limit));

            return new OrderBook(this.Symbol, this.LastUpdateId, this._bids.Take(limit).Select(_ => (_.Key, _.Value.Quantity)), this._asks.Take(limit).Select(_ => (_.Key, _.Value.Quantity)));
        }

        /// <summary>
        /// Get a duplicate order book (deep copy)
        /// </summary>
        /// <returns></returns>
        object ICloneable.Clone() { return this.Clone(); }

        #endregion ICloneable

        #region IEquatable

        public bool Equals(OrderBook other)
        {
            if (other == null)
                return false;

            return other.Symbol == this.Symbol
                && other.LastUpdateId == this.LastUpdateId
                && other.Bids.SequenceEqual(this.Bids)
                && other.Asks.SequenceEqual(this.Asks);
        }

        #endregion IEquatable

        #region Private Classes

        /// <summary>
        /// Comarer used for ordering bids in descending price order.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        private class ReverseComparer<T> : IComparer<T> where T : IComparable<T>
        {
            public int Compare(T x, T y)
            {
                return y.CompareTo(x);
            }
        }

        #endregion Private Classes
    }
    public sealed class OrderBookTop : IEquatable<OrderBookTop>
    {
        #region Public Properties

        /// <summary>
        /// The symbol.
        /// </summary>
        public string Symbol { get; }

        /// <summary>
        /// Best bid price and quantity.
        /// </summary>
        public OrderBookPriceLevel Bid { get; }

        /// <summary>
        /// Best ask price and quantity.
        /// </summary>
        public OrderBookPriceLevel Ask { get; }

        #endregion Public Properties

        #region Constructors

        /// <summary>
        /// Construct order book top.
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="bid">The best bid price and quantity.</param>
        /// <param name="ask">The best ask price and quantity.</param>
        /// <returns></returns>
        public static OrderBookTop Create(string symbol, (decimal, decimal) bid, (decimal, decimal) ask)
            => Create(symbol, bid.Item1, bid.Item2, ask.Item1, ask.Item2);

        /// <summary>
        /// Construct order book top.
        /// </summary>
        /// <param name="symbol">The symbol.</param> 
        /// <param name="bidPrice">The best bid price.</param> 
        /// <param name="bidQuantity">The best bid quantity.</param> 
        /// <param name="askPrice">The best ask price.</param> 
        /// <param name="askQuantity">The best ask quantity.</param> 
        /// <returns></returns>
        public static OrderBookTop Create(string symbol, decimal bidPrice, decimal bidQuantity, decimal askPrice, decimal askQuantity)
        {
            return new OrderBookTop(symbol, new OrderBookPriceLevel(bidPrice, bidQuantity), new OrderBookPriceLevel(askPrice, askQuantity));
        }

        /// <summary>
        /// Construct order book top.
        /// </summary>
        /// <param name="symbol">The symbol.</param>
        /// <param name="bid">The bid price and quantity.</param>
        /// <param name="ask">The ask price and quantity.</param>
        public OrderBookTop(string symbol, OrderBookPriceLevel bid, OrderBookPriceLevel ask)
        {
            Throw.IfNullOrWhiteSpace(symbol, nameof(symbol));
            Throw.IfNull(bid, nameof(bid));
            Throw.IfNull(ask, nameof(ask));

            if (bid.Price > ask.Price)
                throw new ArgumentException($"{nameof(OrderBookTop)} ask price must be greater than the bid price.", nameof(ask.Price));

            this.Symbol = symbol;

            this.Bid = bid;
            this.Ask = ask;
        }

        /// <summary>
        /// Construct order book top.
        /// </summary>
        /// <param name="orderBook">The order book.</param>
        internal OrderBookTop(OrderBook orderBook)
        {
            Throw.IfNull(orderBook, nameof(orderBook));

            this.Symbol = orderBook.Symbol;

            this.Bid = orderBook.Bids.First();
            this.Ask = orderBook.Asks.First();
        }

        #endregion Constructors

        #region IEquatable

        public bool Equals(OrderBookTop other)
        {
            if (other == null)
                return false;

            return other.Symbol == this.Symbol
                && other.Bid.Price == this.Bid.Price
                && other.Bid.Quantity == this.Bid.Quantity
                && other.Ask.Price == this.Ask.Price
                && other.Ask.Quantity == this.Ask.Quantity;
        }

        #endregion IEquatable
    }
}
