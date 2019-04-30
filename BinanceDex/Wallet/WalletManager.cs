using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Numerics;
using System.Threading.Tasks;
using BinanceDex.Api;
using BinanceDex.Api.Models;
using BinanceDex.Utilities;
using BinanceDex.Utilities.Extensions;
using Google.Protobuf;
using NBitcoin;
using NBitcoin.Crypto;
using NBitcoin.DataEncoders;
using Newtonsoft.Json;

namespace BinanceDex.Wallet
{
    public class WalletManager : IWalletManager
    {
        private IBinanceDexApi api;

        public WalletManager(IBinanceDexApi binanceDexApi)
        {
            this.api = binanceDexApi;
        }

        public Task<IWalletManagerResult> CreateWalletAsync(string password)
        {
            return Task.Run<IWalletManagerResult>(() =>
            {
                KeyInformation key = CryptoUtility.GenerateKey(password, this.api.Hrp);

                Wallet wallet = new Wallet
                {
                    IsOpen = true,
                    PrivateKey = key.PrivateKey,
                    Hrp = this.api.Hrp,
                    Address = key.Address,
                    MnemonicWords = key.Mnemonic
                };

                wallet.EcKey = GetEcKey(wallet.PrivateKey);
                wallet.AddressBytes = wallet.EcKey.PubKey.Hash.ToBytes();
                wallet.PubKeyForSign = GetPubKeyForSign(wallet.EcKey.PubKey.ToBytes());

                return new WalletManagerResult()
                {
                    IsSuccessful = true,
                    Wallet = wallet
                };
            });
        }

        public Task<IWalletManagerResult> OpenWalletAsync(string mnemonicWords, string password)
        {
            return Task.Run<IWalletManagerResult>(() =>
            {

                KeyInformation key = CryptoUtility.GetKeyFromMnemonic(mnemonicWords, password, this.api.Hrp);

                Wallet wallet = new Wallet
                {
                    IsOpen = true,
                    PrivateKey = key.PrivateKey,
                    Hrp = this.api.Hrp,
                    EcKey = GetEcKey(key.PrivateKey)
                };
                wallet.Address = key.Address;
                wallet.PubKeyForSign = GetPubKeyForSign(wallet.EcKey.PubKey.ToBytes());
                wallet.MnemonicWords = mnemonicWords;

                //    var api = BinanceApiFactory.CreateApiClient(this.Environment);

                //Account account = await this.api.GetAccountAsync(wallet.Address);
                //if (account != null)
                //{
                //    wallet.AccountNumber = account.AccountNumber;
                //    wallet.Sequence = account.Sequence;
                //}
                //else
                //{
                //    throw new NullReferenceException("Cannot get account information for address " + wallet.Address);
                //}

                //Node nodeInfo = await this.api.GetNodeInfoAsync();
                //if (nodeInfo != null)
                //{
                //    wallet.ChainId = nodeInfo.NodeInfo.Network;
                //}
                //else
                //{
                //    throw new NullReferenceException("Cannot get chain ID");
                //}

                return new WalletManagerResult
                {
                    Wallet = wallet,
                    IsSuccessful = true
                };
            });
        }

        public Task<IWalletManagerResult> OpenWalletAsync(string privateKey)
        {
            return Task.Run<IWalletManagerResult>(() =>
            {
                Wallet wallet = new Wallet
                {
                    IsOpen = true,
                    PrivateKey = privateKey,
                    Hrp = this.api.Hrp,
                    EcKey = GetEcKey(privateKey)
                };

                wallet.AddressBytes = wallet.EcKey.PubKey.Hash.ToBytes();
                wallet.Address = CryptoUtility.GetAddress(wallet.AddressBytes, this.api.Hrp);
                wallet.PubKeyForSign = GetPubKeyForSign(wallet.EcKey.PubKey.ToBytes());
                wallet.MnemonicWords = "";

                return new WalletManagerResult
                {
                    Wallet = wallet,
                    IsSuccessful = true
                };
            });
        }

        public static Key GetEcKey(string privateKey)
        {
            BigInteger privateKeyBigInteger = BigInteger.Parse(privateKey, NumberStyles.HexNumber);
            return new Key(EncodeUtils.HexToBytes(privateKey));
        }

        private static byte[] GetPubKeyForSign(byte[] ecKeyBytes)
        {
            byte[] pubKey = ecKeyBytes;
            byte[] pubKeyPrefix = MessagePrefixes.PubKey;

            byte[] pubKeyForSign = new byte[pubKey.Length + pubKeyPrefix.Length + 1];
            pubKeyPrefix.CopyTo(pubKeyForSign, 0);
            pubKeyForSign[pubKeyPrefix.Length] = 33;
            pubKey.CopyTo(pubKeyForSign, pubKeyPrefix.Length + 1);

            return pubKeyForSign;
        }

    }


    public static class MessagePrefixes
    {
        public static byte[] CancelOrder   => EncodeUtils.HexToBytes("166E681B");
        public static byte[] NewOrder      => EncodeUtils.HexToBytes("CE6DC043");
        public static byte[] TokenFreeze   => EncodeUtils.HexToBytes("E774B32D");
        public static byte[] TokenUnfreeze => EncodeUtils.HexToBytes("6515FF0D");
        public static byte[] Transfer      => EncodeUtils.HexToBytes("2A2C87FA");
        public static byte[] Vote          => EncodeUtils.HexToBytes("A1CADD36");
        public static byte[] PubKey        => EncodeUtils.HexToBytes("EB5AE987");
        public static byte[] StdSignature  => EncodeUtils.HexToBytes("");
        public static byte[] StdTx => new byte[0];
    }

    public class Wallet : IWallet
    {
        public bool IsOpen          { get; internal set; }
        public string PrivateKey    { get; internal set; }
        public string MnemonicWords { get; internal set; }
        public string Address       { get; internal set; }
        public Key EcKey            { get; internal set; }
        public byte[] AddressBytes  { get; internal set; }
        public byte[] PubKeyForSign { get; internal set; }
        public long? AccountNumber  { get; internal set; }
        public long? Sequence       { get; internal set; }
        public string Hrp           { get; internal set; }
        public string ChainId       { get; internal set; }

        public Wallet() { }

        //public override string ToString()
        //{
        //    return new StringBuilder()
        //        .Append($"IsOpen:{IsOpen};")
        //        .Append($"PrivateKey:{PrivateKey};")
        //        .Append($"Address:{Address};")
        //        .Append($"MnemonicWords:{MnemonicWords};")
        //        .Append($"Hrp:{Hrp};")
        //        .Append($"AccountNumber:{AccountNumber};")
        //        .Append($"Sequence:{Sequence};")
        //        .Append($"ChainId:{ChainId};")
        //        .ToString();
        //}

        #region Instance Methods

        public void Close()
        {
            this.IsOpen = false;
            this.PrivateKey = null;
            this.Address = null;
            this.MnemonicWords = null;
            this.Hrp = null;
            this.Sequence = -1;
        }

        public void IncrementSequence()
        {
            this.Sequence++;
        }

        public void EnsureWalletIsReady()
        {
            //if (this.AccountNumber == null || this.Sequence == null)
            //{
            //    InitAccount();
            //}

            //if (this.ChainId == null)
            //{
            //    InitChainId();
            //}
        }

        //private void InitAccount()
        //{
        //    var api = BinanceApiFactory.CreateApiClient(this.Environment);

        //    var account = api.GetAccount(this.Address);
        //    if (account != null)
        //    {
        //        this.AccountNumber = account.AccountNumber;
        //        this.Sequence = account.Sequence;
        //    }
        //    else
        //    {
        //        throw new NullReferenceException("Cannot get account information for address " + this.Address);
        //    }
        //}

        //private void InitChainId()
        //{
        //    var api = BinanceApiFactory.CreateApiClient(this.Environment);

        //    var nodeInfo = api.GetNodeInfo();
        //    if (nodeInfo != null)
        //    {
        //        this.ChainId = nodeInfo.NodeInfo.Network;
        //    }
        //    else
        //    {
        //        throw new NullReferenceException("Cannot get chain ID");
        //    }
        //}

        //#endregion

        //#region Static Private

        //private static Key GetECKey(string privateKey)
        //{
        //    BigInteger privateKeyBigInteger = BigInteger.Parse("0" + privateKey, NumberStyles.HexNumber);
        //    return new Key(privateKeyBigInteger.ToByteArray());
        //}

        //private static byte[] GetPubKeyForSign(byte[] ecKeyBytes)
        //{
        //    byte[] pubKey = ecKeyBytes;
        //    byte[] pubKeyPrefix = MessagePrefixes.PubKey;

        //    byte[] pubKeyForSign = new byte[pubKey.Length + pubKeyPrefix.Length + 1];
        //    pubKeyPrefix.CopyTo(pubKeyForSign, 0);
        //    pubKeyForSign[pubKeyPrefix.Length] = 33;
        //    pubKey.CopyTo(pubKeyForSign, pubKeyPrefix.Length + 1);

        //    return pubKeyForSign;
        //}

        #endregion

    }

    public static class EncodeUtils
    {
        public static byte[] HexToBytes(string hex)
        {
            return Enumerable.Range(0, hex.Length / 2)
                             .Select(x => Convert.ToByte(hex.Substring(x * 2, 2), 16))
                             .ToArray();
        }

        public static byte[] AminoWrap(byte[] raw, byte[] typePrefix, bool isPrefixLength)
        {
            Throw.IfNull(raw, nameof(raw));
            Throw.IfNull(typePrefix, nameof(typePrefix));

            int totalLength = raw.Length + typePrefix.Length;
            ulong totalLengthUlong = (ulong)totalLength;

            if (isPrefixLength) totalLength += CodedOutputStream.ComputeUInt64Size(totalLengthUlong);

            byte[] message = new byte[totalLength];

            using (CodedOutputStream codedOutputStream = new CodedOutputStream(message))
            {
                if (isPrefixLength) codedOutputStream.WriteUInt64(totalLengthUlong);

                foreach (byte t in typePrefix) codedOutputStream.WriteRawTag(t);
                foreach (byte t in raw) codedOutputStream.WriteRawTag(t);

                codedOutputStream.Flush();
                return message;
            }
        }
    }
    public class KeyInformation
    {
        public string Mnemonic { get; set; }
        public string PrivateKey { get; set; }
        public string PublicKey { get; set; }
        public string Address { get; set; }
    }

    public static class CryptoUtility
    {
        public static string GenerateMnemonicCode()
        {
            Mnemonic mnemonic = new Mnemonic(Wordlist.English, WordCount.TwentyFour);
            return mnemonic.ToString();
        }

        public static KeyInformation GenerateKey(string password, string hrp)
        {
            Mnemonic mnemonic = new Mnemonic(Wordlist.English, WordCount.TwentyFour);

            ExtKey extKey = mnemonic.DeriveExtKey(password);
            Key privateKey = extKey.PrivateKey;
            PubKey publicKey = privateKey.PubKey;
            string address = GetAddress(publicKey.Hash.ToBytes(), hrp);

            KeyInformation keyInformation = new KeyInformation
            {
                Address = address,
                Mnemonic = mnemonic.ToString(),
                PrivateKey = privateKey.ToBytes().ToHexString(),
                PublicKey = publicKey.ToString()
            };

            return keyInformation;
        }

        public static KeyInformation GetKeyFromMnemonic(string mnenonicWords, string password, string hrp)
        {
            Mnemonic mnemonic = new Mnemonic(mnenonicWords, Wordlist.English);

            var key = new ExtKey(mnemonic.DeriveSeed(password));
            var extKey = key.Derive(new KeyPath("m/44'/714'/0'/0/0"));

            Key privateKey = extKey.PrivateKey;
            PubKey publicKey = privateKey.PubKey;
            string address = GetAddress(publicKey.Hash.ToBytes(), hrp);

            KeyInformation keyInformation = new KeyInformation
            {
                Address = address,
                Mnemonic = mnemonic.ToString(),
                PrivateKey = privateKey.ToBytes().ToHexString(),
                PublicKey = publicKey.ToString()
            };

            return keyInformation;
        }

        public static byte[] Sign(object data, Key privateKey)
        {
            string serializedData = JsonConvert.SerializeObject(data);
            return Sign(Encoding.UTF8.GetBytes(serializedData), privateKey);
        }

        public static byte[] Sign(byte[] message, Key privateKey)
        {
            byte[] hash;
            using (SHA256 sha256 = SHA256.Create())
            {
                hash = sha256.ComputeHash(message);
            }

            ECDSASignature signature = privateKey.Sign(new uint256(hash, true), false);

            byte[] signatureBytes = new byte[64];
            signature.R.ToByteArrayUnsigned().CopyTo(signatureBytes, 0);
            signature.S.ToByteArrayUnsigned().CopyTo(signatureBytes, 32);

            return signatureBytes;
        }

        public static string GetAddress(byte[] addressBytes, string hrp)
        {
            const string base32Chars = "abcdefghijklmnopqrstuvwxyz234567";

            string base32EncodedData = Encoders.Base32.EncodeData(addressBytes);
            byte[] address32 = new byte[base32EncodedData.Length];

            for (int i = 0; i < base32EncodedData.Length; i++)
            {
                address32[i] = (byte)base32Chars.IndexOf(base32EncodedData[i]);
            }

            Bech32Encoder bech32Encoder = Encoders.Bech32(hrp);
            return bech32Encoder.EncodeData(address32);
        }
    }
}
