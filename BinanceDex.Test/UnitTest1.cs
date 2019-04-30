using System;
using System.Threading.Tasks;
using BinanceDex.Api;
using BinanceDex.Utilities;
using BinanceDex.Wallet;
using ConsoleDump;
using Newtonsoft.Json;
using NUnit.Framework;

namespace BinanceDex.Test
{
    public class BinanceDexApiTests
    {
        public IBinanceDexApi BinanceDexApi { get; set; }

        [SetUp]
        public void Setup()
        {
            this.BinanceDexApi = new BinanceDexApi(new Http(), "https://testnet-dex.binance.org/api/v1/", "tbnb");
        }

        [Test]
        public async Task GetTime()
        {
            (await this.BinanceDexApi.GetTimeAsync()).Dump();
        }

        [Test]
        public async Task GetNodeInfo()
        {
            var result = (await this.BinanceDexApi.GetNodeInfoAsync()).Dump();
            Console.WriteLine(JsonConvert.SerializeObject(result, Formatting.Indented));
        }


        [Test]
        public async Task CreateWallet()
        {
            WalletManager manager = new WalletManager(this.BinanceDexApi);
            var wallet = await  manager.CreateWalletAsync("12345678");

            var result = JsonConvert.SerializeObject(wallet.Wallet, Formatting.Indented, new JsonSerializerSettings{MaxDepth=1,ReferenceLoopHandling=ReferenceLoopHandling.Ignore,Error= (sender, args) => args.Dump()});

            Console.WriteLine(result);
        }

    }
}