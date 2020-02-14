using System;
using System.Collections.Generic;
using System.IO;
using BinanceDex.Utilities.Extensions;
using BinanceDex.Wallet;
using Newtonsoft.Json;
using ProtoBuf;

namespace BinanceDex.Api.BroadcastModels
{
    public class StdSignatureProto
    {
        [ProtoMember(1)]
        public byte[] PubKey { get; set; }

        [ProtoMember(2)]
        public byte[] Signature { get; set; }

        [ProtoMember(3)]
        public long AccountNumber { get; set; }

        [ProtoMember(4)]
        public long Sequence { get; set; }
    }
    public class TransactionOption 
    {
        [JsonProperty("memo")]
        public string Memo { get; set; }

        [JsonProperty("source")]
        public long Source { get; set; }

        [JsonProperty("data")]
        public byte[] Data { get; set; }
    }
    internal class SignData<T>
    {
        [JsonProperty("chain_id")]
        public string ChainId { get; set; }

        [JsonProperty("account_number")]
        public string AccountNumber { get; set; }

        [JsonProperty("sequence")]
        public string Sequence { get; set; }

        [JsonProperty("memo")]
        public string Memo { get; set; }

        [JsonProperty("msgs")]
        public List<T> Messages { get; set; }

        [JsonProperty("source")]
        public long Source { get; set; }

        [JsonProperty("data")]
        public byte[] Data { get; set; }
    }
    internal class BroadcastBase
    {
        //public abstract string BuildMessageBody();

        public long StringDecimalToLong(string value)
        {
            decimal MULTIPLY_FACTOR = 1e8M;
            decimal encodeValue = decimal.Multiply(Convert.ToDecimal(value), MULTIPLY_FACTOR);
            if (encodeValue.CompareTo(decimal.Zero) <= 0)
            {
                throw new ArgumentException(value + " is less or equal to zero.");
            }
            if (encodeValue.CompareTo(long.MaxValue) > 0)
            {
                throw new ArgumentException(value + " is too large.");
            }
            return Convert.ToInt64(encodeValue);
        }

        public string Bytess3ToHex(byte[] bytes)
        {
            return bytes.ToHexString();
        }

        //public byte[] Sign<T>(T message, Wallet wallet, TransactionOption options) where T:class
        //{
        //    SignData<T> signData = new SignData<T>();
        //    signData.ChainId = wallet.ChainId;
        //    signData.AccountNumber = wallet.AccountNumber.ToString();
        //    signData.Sequence = wallet.Sequence.ToString();
        //    signData.Messages = new List<T>() { message };
        //    signData.Memo = options.Memo;
        //    signData.Source = options.Source;
        //    signData.Data = options.Data;

        //    return CryptoUtility.Sign(signData, wallet.EcKey);
        //}

        //public byte[] EncodeSignature(byte[] signature, Wallet wallet, TransactionOption options)
        //{
        //    StdSignatureProto stdSignature = new StdSignatureProto
        //    {
        //        PubKey = wallet.PubKeyForSign,
        //        Signature = signature,
        //        AccountNumber = wallet.AccountNumber.Value,
        //        Sequence = wallet.Sequence.Value
        //    };

        //    return this.EncodeMessage<StdSignatureProto>(stdSignature, MessagePrefixes.StdSignature);
        //}

        //public byte[] EncodeStandardTx(byte[] message, byte[] signature, TransactionOption options)
        //{
        //    StdTxProto stdTx = new StdTxProto()
        //    {
        //        Msgs = message,
        //        Signatures = signature,
        //        Memo = options.Memo,
        //        Source = options.Source,
        //        Data = options.Data != null ? options.Data : null
        //    };

        //    return EncodeMessage<StdTxProto>(stdTx, MessagePrefixes.StdTx);
        //}

        public byte[] EncodeMessage<T>(T message, byte[] prefix) where T : class
        {
            return EncodeUtils.AminoWrap(this.ProtoSerialize(message), prefix, false);
        }

        private byte[] ProtoSerialize<T>(T record) where T : class
        {
            if (record == null)
                return null;

            using (MemoryStream stream = new MemoryStream())
            {
                Serializer.Serialize(stream, record);
                return stream.ToArray();
            }
        }

        private T ProtoDeserialize<T>(byte[] data) where T : class
        {
            if (data == null)
                return null;

            using (MemoryStream stream = new MemoryStream(data))
            {
                return Serializer.Deserialize<T>(stream);
            }
        }


    }
}
