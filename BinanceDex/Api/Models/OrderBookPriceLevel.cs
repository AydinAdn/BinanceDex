using System;
using BinanceDex.Utilities.JsonConverters;
using Newtonsoft.Json;

namespace BinanceDex.Api.Models
{
    [JsonConverter(typeof(OrderBookPriceLevelJsonConverter))]
    public sealed class OrderBookPriceLevel : IEquatable<OrderBookPriceLevel>
    {
        #region Constructors

        /// <summary>
        ///     Construct order book level.
        /// </summary>
        /// <param name="price">The price.</param>
        /// <param name="quantity">The quantity.</param>
        public OrderBookPriceLevel(decimal price, decimal quantity)
        {
            if (price < 0)    throw new ArgumentException($"{nameof(OrderBookPriceLevel)} price must greater than or equal to 0.", nameof(price));
            if (quantity < 0) throw new ArgumentException($"{nameof(OrderBookPriceLevel)} quantity must be greater than or equal to 0.", nameof(quantity));

            this.Price = price;
            this.Quantity = quantity;
        }

        #endregion

        #region Properties

        /// <summary>
        ///     Get the price.
        /// </summary>
        public decimal Price { get; }

        /// <summary>
        ///     Get the quantity.
        /// </summary>
        public decimal Quantity { get; internal set; }

        #endregion

        #region IEquatable

        public bool Equals(OrderBookPriceLevel other)
        {
            if (other == null) return false;

            return other.Price == this.Price && other.Quantity == this.Quantity;
        }

        #endregion IEquatable
    }
}