using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using BinanceDex.Api.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace BinanceDex.Utilities.JsonConverters
{
    public class OrderBookPriceLevelJsonConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(OrderBookPriceLevel);
        }

        [SuppressMessage("ReSharper", "PossibleMultipleEnumeration")]
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            JToken token = JToken.Load(reader);

            if (token.Type != JTokenType.Array) return null;

            IEnumerable<decimal> value = token.SelectToken(token.Path)
                                              .Select(t => t.Value<decimal>());

            return new OrderBookPriceLevel(value.ElementAt(0), value.ElementAt(1));

        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }

    }
}