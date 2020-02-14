using BinanceDex.Api;
using BinanceDex.Utilities;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BinanceDex.Api.Models;
using ConsoleDump;
using Newtonsoft.Json;
using Times = BinanceDex.Api.Models.Times;

namespace BinanceDex.Test.Api
{
    [TestFixture]
    public class BinanceDexApiTests
    {
        private MockRepository mockRepository;

        private Mock<IHttp> mockHttp;

        [SetUp]
        public void SetUp()
        {
            this.mockRepository = new MockRepository(MockBehavior.Strict);

            this.mockHttp = this.mockRepository.Create<IHttp>();
        }

        [TearDown]
        public void TearDown()
        {
            //this.mockRepository.VerifyAll();
        }

        private BinanceDexApi CreateBinanceDexApi()
        {
            this.mockHttp.Setup(x=>x.GetAsync("https://testnet-dex.binance.org/api/v1/time"))     .ReturnsAsync(new HttpResponse{Response = @"{""ap_time"":""2019-04-24T10:28:27Z"",""block_time"":""2019-04-24T10:28:26Z""}", StatusCode = 200}).Verifiable();
            this.mockHttp.Setup(x=>x.GetAsync("https://testnet-dex.binance.org/api/v1/node-info")).ReturnsAsync(new HttpResponse{Response = @"{""node_info"":{""id"":""c4d94f29e765ecfe81c940e11c2e997321aa8e0f"",""listen_addr"":""10.203.43.118:27146"",""network"":""Binance-Chain-Nile"",""version"":""0.30.1"",""channels"":""3540202122233038"",""moniker"":""Zugspitze"",""other"":{""amino_version"":"""",""p2p_version"":"""",""consensus_version"":"""",""rpc_version"":"""",""tx_index"":""on"",""rpc_address"":""tcp://0.0.0.0:27147""}},""sync_info"":{""latest_block_hash"":""DC18585214F618159722EEEB51C5973800705A490A4D2EB2F48413EF2B90ED6D"",""latest_app_hash"":""39DB053E04AA1683F9ED832C4EDE65CC35D38BB44E53303ED919F1BFC0C7F08C"",""latest_block_height"":10385258,""latest_block_time"":""2019-04-24T11:02:12.664627443Z"",""catching_up"":false},""validator_info"":{""address"":""91844D296BD8E591448EFC65FD6AD51A888D58FA"",""pub_key"":[200,14,154,190,247,255,67,156,16,198,143,232,241,48,61,237,223,197,39,113,140,59,55,216,186,104,7,68,110,60,130,122],""voting_power"":100000000000}}", StatusCode = 200});
            this.mockHttp.Setup(x=>x.GetAsync("https://testnet-dex.binance.org/api/v1/validators")).ReturnsAsync(new HttpResponse{Response = @"{""block_height"":10387667,""validators"":[{""address"":""06FD60078EB4C2356137DD50036597DB267CF616"",""pub_key"":[22,36,222,100,32,225,124,190,156,32,205,207,223,135,107,59,18,151,141,50,100,160,7,252,170,167,28,76,219,112,29,158,188,3,35,244,79],""voting_power"":100000000000},{""address"":""18E69CC672973992BB5F76D049A5B2C5DDF77436"",""pub_key"":[22,36,222,100,32,24,78,123,16,61,52,196,16,3,249,184,100,213,248,193,173,218,155,208,67,107,37,59,179,200,68,188,115,156,30,119,201],""voting_power"":100000000000},{""address"":""344C39BB8F4512D6CAB1F6AAFAC1811EF9D8AFDF"",""pub_key"":[22,36,222,100,32,77,66,10,234,132,62,146,160,207,230,157,137,105,109,255,104,39,118,159,156,181,42,36,154,245,55,206,137,191,42,75,116],""voting_power"":100000000000},{""address"":""37EF19AF29679B368D2B9E9DE3F8769B35786676"",""pub_key"":[22,36,222,100,32,189,3,222,159,138,178,158,40,0,9,78,21,63,172,111,105,108,250,81,37,54,201,194,248,4,220,178,194,196,228,174,214],""voting_power"":100000000000},{""address"":""62633D9DB7ED78E951F79913FDC8231AA77EC12B"",""pub_key"":[22,36,222,100,32,143,74,116,160,115,81,137,93,223,55,48,87,185,143,174,109,250,242,205,33,243,122,6,62,25,96,16,120,254,71,13,83],""voting_power"":100000000000},{""address"":""7B343E041CA130000A8BC00C35152BD7E7740037"",""pub_key"":[22,36,222,100,32,74,93,71,83,235,121,249,46,128,239,226,45,247,172,164,246,102,164,244,75,248,28,83,108,74,9,212,185,197,182,84,181],""voting_power"":100000000000},{""address"":""91844D296BD8E591448EFC65FD6AD51A888D58FA"",""pub_key"":[22,36,222,100,32,200,14,154,190,247,255,67,156,16,198,143,232,241,48,61,237,223,197,39,113,140,59,55,216,186,104,7,68,110,60,130,122],""voting_power"":100000000000},{""address"":""B3727172CE6473BC780298A2D66C12F1A14F5B2A"",""pub_key"":[22,36,222,100,32,145,66,175,204,105,27,124,192,93,38,199,176,190,12,139,70,65,130,148,23,23,48,224,121,243,132,253,226,250,80,186,252],""voting_power"":100000000000},{""address"":""B6F20C7FAA2B2F6F24518FA02B71CB5F4A09FBA3"",""pub_key"":[22,36,222,100,32,73,178,136,228,235,187,58,40,28,45,84,111,195,2,83,213,186,240,137,147,182,229,210,149,251,120,122,91,49,74,41,142],""voting_power"":100000000000},{""address"":""E0DD72609CC106210D1AA13936CB67B93A0AEE21"",""pub_key"":[22,36,222,100,32,4,34,67,57,104,143,1,46,100,157,228,142,36,24,128,9,46,170,143,106,160,244,241,75,252,249,224,199,105,23,192,182],""voting_power"":100000000000},{""address"":""FC3108DC3814888F4187452182BC1BAF83B71BC9"",""pub_key"":[22,36,222,100,32,64,52,179,124,237,168,160,191,19,177,171,174,238,122,143,147,131,84,32,153,165,84,210,25,185,61,12,230,158,57,112,232],""voting_power"":100000000000}]}" }) ;
            this.mockHttp.Setup(x=>x.GetAsync("https://testnet-dex.binance.org/api/v1/peers")).ReturnsAsync(new HttpResponse{Response = @"[{""accelerated"":true,""access_addr"":""https://testnet-dex.binance.org:443"",""capabilities"":[""qs"",""ap"",""ws""],""id"":""gateway-ingress"",""listen_addr"":""https://testnet-dex.binance.org:443"",""moniker"":""gateway-ingress"",""network"":""gateway"",""stream_addr"":""wss://testnet-dex.binance.org"",""version"":""1.0.0""},{""access_addr"":""http://seed-pre-s1.binance.org:80"",""capabilities"":[""node""],""id"":""aea74b16d28d06cbfbb1179c177e8cd71315cce4"",""listen_addr"":""http://seed-pre-s1.binance.org:80"",""moniker"":""seed"",""network"":""Binance-Chain-Nile"",""original_listen_addr"":""ac6d84c3f243a11e98ced0ac108d49f7-704ea117aa391bbe.elb.ap-northeast-1.amazonaws.com:27146"",""version"":""0.30.1""},{""access_addr"":""https://data-seed-pre-0-s1.binance.org:443"",""capabilities"":[""node""],""id"":""9612b570bffebecca4776cb4512d08e252119005"",""listen_addr"":""https://data-seed-pre-0-s1.binance.org:443"",""moniker"":""data-seed-0"",""network"":""Binance-Chain-Nile"",""original_listen_addr"":""a0b88b324243a11e994280efee3352a7-96b6996626c6481d.elb.ap-northeast-1.amazonaws.com:27146"",""version"":""0.30.1""},{""access_addr"":""https://data-seed-pre-1-s1.binance.org:443"",""capabilities"":[""node""],""id"":""8c379d4d3b9995c712665dc9a9414dbde5b30483"",""listen_addr"":""https://data-seed-pre-1-s1.binance.org:443"",""moniker"":""data-seed-1"",""network"":""Binance-Chain-Nile"",""original_listen_addr"":""aa1e4d0d1243a11e9a951063f6065739-7a82be90a58744b6.elb.ap-northeast-1.amazonaws.com:27146"",""version"":""0.30.1""},{""access_addr"":""https://data-seed-pre-2-s1.binance.org:443"",""capabilities"":[""node""],""id"":""7156d461742e2a1e569fd68426009c4194830c93"",""listen_addr"":""https://data-seed-pre-2-s1.binance.org:443"",""moniker"":""data-seed-2"",""network"":""Binance-Chain-Nile"",""original_listen_addr"":""aa841c226243a11e9a951063f6065739-eee556e439dc6a3b.elb.ap-northeast-1.amazonaws.com:27146"",""version"":""0.30.1""}]" }) ;
            this.mockHttp.Setup(x=>x.GetAsync("https://testnet-dex.binance.org/api/v1/account/tbnb1zllq4cyrztmpxt7h4k92f8gpqkgmzwqpt82rek")).ReturnsAsync(new HttpResponse{Response = @"{""address"":""tbnb1zllq4cyrztmpxt7h4k92f8gpqkgmzwqpt82rek"",""public_key"":[2,254,13,248,102,223,188,250,33,144,165,201,140,152,90,225,176,202,206,133,8,122,54,124,147,136,88,102,135,91,140,238,158],""account_number"":667861,""sequence"":1,""balances"":[{""symbol"":""BNB"",""free"":""149.98000000"",""locked"":""0.00000000"",""frozen"":""0.00000000""},{""symbol"":""BTC.B-918"",""free"":""0.21940226"",""locked"":""0.00000000"",""frozen"":""0.00000000""}]}" }) ;
            this.mockHttp.Setup(x=>x.GetAsync("https://testnet-dex.binance.org/api/v1/tx/")).ReturnsAsync(new HttpResponse{Response = @"{""data"":""Tx{DD01F0625DEE0A65CE6DC0430A1417FE0AE08312F6132FD7AD8AA49D010591B13801122A313746453041453038333132463631333246443741443841413439443031303539314231333830312D311A0D424E425F4254432E422D3931382002280230A5E21A3880E497D0124001126E0A26EB5AE9872102FE0DF866DFBCFA2190A5C98C985AE1B0CACE85087A367C93885866875B8CEE9E124051E322E820D0F182D43DA746ACEBA4D173BCE8917F7267780F98A62E5249A11554C625316E7466A510A723FA53B85F104690999C7C3DCF58CEE0426CEEC4855E18D5E1282001}"",""hash"":""\u0015\u0013\ufffd\""ˏʦ\ufffd\rK\ufffdaE\ufffd\u0018\ufffd\ufffd\ufffd\u0010"",""log"":""Msg 0: "",""ok"":true}" }) ;
            this.mockHttp.Setup(x=>x.GetAsync("https://testnet-dex.binance.org/api/v1/tokens")).ReturnsAsync(new HttpResponse{Response = @"[{""name"":""binance coin of he yi"",""symbol"":""000-0E1"",""original_symbol"":""000"",""total_supply"":""100.00000000"",""owner"":""tbnb1j9jm8uc54scme3z6kkl6snjja9fkljx06hqdu8"",""mintable"":true},{""name"":""binance coin of he yi"",""symbol"":""000-A3A"",""original_symbol"":""000"",""total_supply"":""100.00000000"",""owner"":""tbnb1j9jm8uc54scme3z6kkl6snjja9fkljx06hqdu8"",""mintable"":true},{""name"":""Triple Zero"",""symbol"":""000-EF6"",""original_symbol"":""000"",""total_supply"":""100000.00000000"",""owner"":""tbnb1ysyrec2zk9rywut077q00qymprrsav9xpgt60n"",""mintable"":false},{""name"":""000A Token"",""symbol"":""000A-41D"",""original_symbol"":""000A"",""total_supply"":""10000000000.00000000"",""owner"":""tbnb1a5ev89sj83hd7xg0zdssksdfz7wenw4le9jy94"",""mintable"":false},{""name"":""Triple Zero Coin"",""symbol"":""000C-11D"",""original_symbol"":""000C"",""total_supply"":""1000000.00000000"",""owner"":""tbnb1gl554h7l384tl2cxafpcysak28hp62n74h3g3p"",""mintable"":false},{""name"":""binance coin of he yi"",""symbol"":""001-B44"",""original_symbol"":""001"",""total_supply"":""100.00000000"",""owner"":""tbnb1j9jm8uc54scme3z6kkl6snjja9fkljx06hqdu8"",""mintable"":true},{""name"":""binance coin of he yi"",""symbol"":""001-FD7"",""original_symbol"":""001"",""total_supply"":""100.00000000"",""owner"":""tbnb1j9jm8uc54scme3z6kkl6snjja9fkljx06hqdu8"",""mintable"":true},{""name"":""007 Token"",""symbol"":""007-749"",""original_symbol"":""007"",""total_supply"":""10000.00000000"",""owner"":""tbnb1ysyrec2zk9rywut077q00qymprrsav9xpgt60n"",""mintable"":false},{""name"":""0KI Token"",""symbol"":""0KI-0AF"",""original_symbol"":""0KI"",""total_supply"":""102000.00000000"",""owner"":""tbnb1ycg4yy2hdjd062z2h43e86fgq6atf0dpnram7f"",""mintable"":false},{""name"":""0NE Token"",""symbol"":""0NE-AF3"",""original_symbol"":""0NE"",""total_supply"":""10000000000.00000000"",""owner"":""tbnb1vlnatgu05rd4993e7m2waaqnel739mcwq3fee5"",""mintable"":false},{""name"":""100K TOKEN Not Mintable"",""symbol"":""100K-9BC"",""original_symbol"":""100K"",""total_supply"":""100000.00000000"",""owner"":""tbnb1228u39qa7gv5373dg00d2m4y7aypupx2cp66mc"",""mintable"":false},{""name"":""Only 10000 Tokens NOT MINTABLE"",""symbol"":""10KONLY-2C1"",""original_symbol"":""10KONLY"",""total_supply"":""10000.00000000"",""owner"":""tbnb19m70v0u2ppdnklpv607l3cgm5fqn2j9n3r4j09"",""mintable"":false},{""name"":""Only 10000 Tokens NOT MINTABLE"",""symbol"":""10KONLY-351"",""original_symbol"":""10KONLY"",""total_supply"":""1000.00000000"",""owner"":""tbnb19m70v0u2ppdnklpv607l3cgm5fqn2j9n3r4j09"",""mintable"":false},{""name"":""xuNsh1ne"",""symbol"":""1337-1A9"",""original_symbol"":""1337"",""total_supply"":""1000000000.00000000"",""owner"":""tbnb1vdvrpze3rt6r0fhchwnzpsnpjqnmh4nw09m94a"",""mintable"":true},{""name"":""Elite Will Moon Last"",""symbol"":""1337MOON-B51"",""original_symbol"":""1337MOON"",""total_supply"":""90000000000.00000000"",""owner"":""tbnb17ynkkkxh92fqn9p9upd4nd5d0u9yal8lpxqa0d"",""mintable"":false},{""name"":""1k Supply Experiment Volume Coin"",""symbol"":""1KVOLUME-736"",""original_symbol"":""1KVOLUME"",""total_supply"":""1000.00000000"",""owner"":""tbnb18t67xvg9jwjl4v02akpusak05fhq2qf4qc2w5t"",""mintable"":false},{""name"":""1k Supply Experiment Volume Coin"",""symbol"":""1KVOLUME-D65"",""original_symbol"":""1KVOLUME"",""total_supply"":""80000001000.00000000"",""owner"":""tbnb18t67xvg9jwjl4v02akpusak05fhq2qf4qc2w5t"",""mintable"":true},{""name"":""'\u003cimg\u003e"",""symbol"":""2332-1BF"",""original_symbol"":""2332"",""total_supply"":""1000.00000000"",""owner"":""tbnb1nlwncf6wfwm7hw9s8kujfuf5jq2p08fyjpzw7w"",""mintable"":true},{""name"":""80DASHOU"",""symbol"":""80DASHOU-729"",""original_symbol"":""80DASHOU"",""total_supply"":""100010000.00000000"",""owner"":""tbnb1hjvyltdgc5kks6a9fatd89au2j6hhtev3c225a"",""mintable"":true},{""name"":""81JIANJUN"",""symbol"":""81JIAN-3E8"",""original_symbol"":""81JIAN"",""total_supply"":""100010000.00000000"",""owner"":""tbnb1wcs3tcn3hh4q9mvjhq5e5x7glrqsw9cxedjgh7"",""mintable"":true},{""name"":""82XIYOU"",""symbol"":""82XIYOU-34D"",""original_symbol"":""82XIYOU"",""total_supply"":""100010000.00000000"",""owner"":""tbnb1qzpyce084aj9eguqyqwdmz7kvs8fqz90lu0d56"",""mintable"":true},{""name"":""83SHUIHU"",""symbol"":""83SHUIHU-AC4"",""original_symbol"":""83SHUIHU"",""total_supply"":""100010000.00000000"",""owner"":""tbnb1lguzx55tkn94kyjkcpvkav78zj50cv7qdzm9al"",""mintable"":true},{""name"":""83SHEDIAO"",""symbol"":""84SHEDAO-F6F"",""original_symbol"":""84SHEDAO"",""total_supply"":""100010000.00000000"",""owner"":""tbnb10crx0zx58atrz8gu9qf5dtt08hp0r4cx2qj7fy"",""mintable"":true},{""name"":""85SHEDIAO"",""symbol"":""85DUC-800"",""original_symbol"":""85DUC"",""total_supply"":""200010000.00000000"",""owner"":""tbnb1gwv738uvydscqffqgqw4rsfn50sm8ypsu2k6yq"",""mintable"":true},{""name"":""MEIWENTI"",""symbol"":""86OK-B90"",""original_symbol"":""86OK"",""total_supply"":""100010000.00000000"",""owner"":""tbnb1ahkt4q2wfrquyfpjju6uc2t0hkak72wp9qhktq"",""mintable"":true},{""name"":""MEIWENTI"",""symbol"":""87KEYI-248"",""original_symbol"":""87KEYI"",""total_supply"":""100010000.00000000"",""owner"":""tbnb15ekcrp2cs54sce7re8jhrmzhfu42nh5rnn32hu"",""mintable"":true},{""name"":""MEIWENTI"",""symbol"":""8888-E6D"",""original_symbol"":""8888"",""total_supply"":""100010000.00000000"",""owner"":""tbnb17zvfykec227d8rxteymxqmnff3rahdmtlrsf2f"",""mintable"":true},{""name"":""8989"",""symbol"":""8989-4DC"",""original_symbol"":""8989"",""total_supply"":""100010000.00000000"",""owner"":""tbnb1wu2a2yg6d3fd9h7reetel07eg5xgcs5k42nz4f"",""mintable"":true},{""name"":""AAA"",""symbol"":""AAA-25F"",""original_symbol"":""AAA"",""total_supply"":""1000000000.00000000"",""owner"":""tbnb1jyyl59pnduzadyx28fv9ed3jqeczzeskqrchkg"",""mintable"":true},{""name"":""“BATTERY”"",""symbol"":""AAA-B50"",""original_symbol"":""AAA"",""total_supply"":""1000000000.00000000"",""owner"":""tbnb1ejfew3fqula4cj9tux7kzm9mc7we8qfu94ess5"",""mintable"":true},{""name"":""AAA insurance coin"",""symbol"":""AAA-EB8"",""original_symbol"":""AAA"",""total_supply"":""200000000.00000000"",""owner"":""tbnb1zkpj55vl8ntc849wgr2x6cw8rw5jq52e582fry"",""mintable"":false},{""name"":""The Official AAAAAA Token"",""symbol"":""AAAAAA-BBA"",""original_symbol"":""AAAAAA"",""total_supply"":""5000010000.00000000"",""owner"":""tbnb19m70v0u2ppdnklpv607l3cgm5fqn2j9n3r4j09"",""mintable"":true},{""name"":""Triple A BNB Token"",""symbol"":""AAABNB-3B6"",""original_symbol"":""AAABNB"",""total_supply"":""7001000001.00000000"",""owner"":""tbnb19m70v0u2ppdnklpv607l3cgm5fqn2j9n3r4j09"",""mintable"":true},{""name"":""Madeira AAD"",""symbol"":""AAD-E18"",""original_symbol"":""AAD"",""total_supply"":""1000000000.00000000"",""owner"":""tbnb1p7dnxxn9gh5v67ud9d56fjcplumcr4hz83503g"",""mintable"":true},{""name"":""AAS token"",""symbol"":""AAS-361"",""original_symbol"":""AAS"",""total_supply"":""1000000000.00000000"",""owner"":""tbnb1sl285558t0w3n6mpkrwseh9cjw5gcecpthcx9d"",""mintable"":true},{""name"":""american business coin"",""symbol"":""ABC-ACC"",""original_symbol"":""ABC"",""total_supply"":""1000000000.00000000"",""owner"":""tbnb1jy4culmgkphdgtpw22l3gehpkx8aly0zjms98j"",""mintable"":true},{""name"":""A Official BTC of BNB"",""symbol"":""ABNB-919"",""original_symbol"":""ABNB"",""total_supply"":""1000000000.00000000"",""owner"":""tbnb19l3fstvdu0rnv382z3fp37usx9en5z4eu4e707"",""mintable"":true},{""name"":""Ace Club"",""symbol"":""ACE-A6F"",""original_symbol"":""ACE"",""total_supply"":""20000000.00000000"",""owner"":""tbnb186p0dlnujcqlalkr36s5sccwnnww0juamdwl23"",""mintable"":false},{""name"":""ace token"",""symbol"":""ACE-C14"",""original_symbol"":""ACE"",""total_supply"":""1000000000.00000000"",""owner"":""tbnb1clyn0ky8dm5gx35tsnntzzvylzfxacqj4dttq9"",""mintable"":true},{""name"":""first ace"",""symbol"":""ACE-E57"",""original_symbol"":""ACE"",""total_supply"":""1000000000.00000000"",""owner"":""tbnb1lx6ufgmgh73ft04xw7lte6rkfgu8gkt7c2ywmg"",""mintable"":true},{""name"":""Agricola coin"",""symbol"":""AGRI-BD2"",""original_symbol"":""AGRI"",""total_supply"":""1000000000.00000000"",""owner"":""tbnb1jwx3ga9cfccmds3gyax3w4x65qqva62t297g7a"",""mintable"":true},{""name"":""ALIS"",""symbol"":""ALIS-95B"",""original_symbol"":""ALIS"",""total_supply"":""10000.00000000"",""owner"":""tbnb1jlth0myl2t65gplrud27sjycehm97e0vcujazf"",""mintable"":true},{""name"":""ANN Network"",""symbol"":""ANN-457"",""original_symbol"":""ANN"",""total_supply"":""100000000.00000000"",""owner"":""tbnb14zguq8gf58ms07npae7pluqxm27xvvgftmhsxz"",""mintable"":true},{""name"":""Ante"",""symbol"":""ANTE-E5D"",""original_symbol"":""ANTE"",""total_supply"":""1000000000.00000000"",""owner"":""tbnb1kunyuls69p5yynem5t57dt6rpat5pdc384d2e7"",""mintable"":true},{""name"":""apple coin"",""symbol"":""APP-69D"",""original_symbol"":""APP"",""total_supply"":""90000000000.00000000"",""owner"":""tbnb128rljlszhyy7334yjna6h3q8sfptgwy5udwt8c"",""mintable"":false},{""name"":""“Aeron”"",""symbol"":""ARN-394"",""original_symbol"":""ARN"",""total_supply"":""30000000.00000000"",""owner"":""tbnb1mwkj2lg8qqel80k6l5gscacyyxv3l4y6evn5yz"",""mintable"":true},{""name"":""“SAMSUNG"",""symbol"":""ASA-DC5"",""original_symbol"":""ASA"",""total_supply"":""1000000000.00000000"",""owner"":""tbnb1ejfew3fqula4cj9tux7kzm9mc7we8qfu94ess5"",""mintable"":true},{""name"":""Astro Coin"",""symbol"":""ASTRO-F7B"",""original_symbol"":""ASTRO"",""total_supply"":""150000000.00000000"",""owner"":""tbnb1553at9gn0wg20ydyaxya7a6md6y4k09tjxuuwc"",""mintable"":false},{""name"":""The Currency of Life"",""symbol"":""ATP-923"",""original_symbol"":""ATP"",""total_supply"":""90000000000.00000000"",""owner"":""tbnb1ph20ccf30vht2zzneywmagx056dw4p2vrpcty4"",""mintable"":true},{""name"":""Attention Token"",""symbol"":""ATT-E43"",""original_symbol"":""ATT"",""total_supply"":""42000000.00000000"",""owner"":""tbnb12vefyyx3s8ld02uphghkftk3tdedp7tytye8ws"",""mintable"":true},{""name"":""Advanced Volatility Token"",""symbol"":""AVT-B74"",""original_symbol"":""AVT"",""total_supply"":""250000.00000000"",""owner"":""tbnb109hnwk9zcncq767mxfuakq509drd5hderm889d"",""mintable"":true},{""name"":""“Binance"",""symbol"":""BAC-F05"",""original_symbol"":""BAC"",""total_supply"":""100000.00000000"",""owner"":""tbnb183r6rdqk3fgm98kvkwnqjjfag0lqm6xt4a5p2g"",""mintable"":false},{""name"":""Bianana Token"",""symbol"":""BAT-4B1"",""original_symbol"":""BAT"",""total_supply"":""10000.00000000"",""owner"":""tbnb10qcayfntt0r97tt2sgpyjfhx3uyc43mdarvmwa"",""mintable"":false},{""name"":""Binance Community Australia"",""symbol"":""BAU-7D7"",""original_symbol"":""BAU"",""total_supply"":""100000.00000000"",""owner"":""tbnb183r6rdqk3fgm98kvkwnqjjfag0lqm6xt4a5p2g"",""mintable"":false},{""name"":""BAZZA"",""symbol"":""BAZ-14F"",""original_symbol"":""BAZ"",""total_supply"":""1.00000000"",""owner"":""tbnb1h46g04t9yh9s4eyzynxxr83qefcpwnauwhqqqe"",""mintable"":false},{""name"":""BAZZAA"",""symbol"":""BAZ-9DA"",""original_symbol"":""BAZ"",""total_supply"":""100000000.00000000"",""owner"":""tbnb1h46g04t9yh9s4eyzynxxr83qefcpwnauwhqqqe"",""mintable"":false},{""name"":""Binance Balance"",""symbol"":""BBC-D4D"",""original_symbol"":""BBC"",""total_supply"":""100000.00000000"",""owner"":""tbnb183r6rdqk3fgm98kvkwnqjjfag0lqm6xt4a5p2g"",""mintable"":false},{""name"":""Bit Coin"",""symbol"":""BC1-3A1"",""original_symbol"":""BC1"",""total_supply"":""10000.00000000"",""owner"":""tbnb1e7k90ns8j7nsstcnrrys07wf8c0w2ye0nvlvtq"",""mintable"":true},{""name"":""Bit Coin"",""symbol"":""BC1-7C2"",""original_symbol"":""BC1"",""total_supply"":""10000.00000000"",""owner"":""tbnb1e7k90ns8j7nsstcnrrys07wf8c0w2ye0nvlvtq"",""mintable"":true},{""name"":""bey token"",""symbol"":""BEY-8C6"",""original_symbol"":""BEY"",""total_supply"":""20000.00000000"",""owner"":""tbnb1d68mhlhyr6y7cjlfnhl7shfawgcnqcxkag7p40"",""mintable"":true},{""name"":""BingeRun"",""symbol"":""BIN-986"",""original_symbol"":""BIN"",""total_supply"":""100000000.00000000"",""owner"":""tbnb1q64fmem88sp9nlzn6epjyz0pnuga85uw0wsh0d"",""mintable"":true},{""name"":""Show Some ♥ for Binance"",""symbol"":""BINANCE-1DB"",""original_symbol"":""BINANCE"",""total_supply"":""90000000000.00000000"",""owner"":""tbnb1u37ysv75tv8ehjw700nwnu263edde5nrw0l548"",""mintable"":false},{""name"":""Show Some ♥ for Binance"",""symbol"":""BINANCE-DCE"",""original_symbol"":""BINANCE"",""total_supply"":""10000.00000000"",""owner"":""tbnb1u37ysv75tv8ehjw700nwnu263edde5nrw0l548"",""mintable"":false},{""name"":""Blue Lambo Coin"",""symbol"":""BLC-2E7"",""original_symbol"":""BLC"",""total_supply"":""1000000.00000000"",""owner"":""tbnb1cjzczgyj85dky4mgcwe7m5tcl8lgglmw8y3u3j"",""mintable"":true},{""name"":""BLIS"",""symbol"":""BLIS-2FC"",""original_symbol"":""BLIS"",""total_supply"":""10000.00000000"",""owner"":""tbnb1jlth0myl2t65gplrud27sjycehm97e0vcujazf"",""mintable"":true},{""name"":""Bolenum"",""symbol"":""BLN-96C"",""original_symbol"":""BLN"",""total_supply"":""25000000000.00000000"",""owner"":""tbnb1gagygkjmj6u7dvkapu0lufpdyhsgc7epcjl53g"",""mintable"":true},{""name"":""Bambus Coin"",""symbol"":""BMB-6AC"",""original_symbol"":""BMB"",""total_supply"":""12000000.00000000"",""owner"":""tbnb1qwvaq5sup5ukylj0zzuvgun0s8v58sd265agmt"",""mintable"":false},{""name"":""Binance Chain Native Token"",""symbol"":""BNB"",""original_symbol"":""BNB"",""total_supply"":""200000000.00000000"",""owner"":""tbnb12hlquylu78cjylk5zshxpdj6hf3t0tahwjt3ex"",""mintable"":false},{""name"":""BNN Network"",""symbol"":""BNN-411"",""original_symbol"":""BNN"",""total_supply"":""100000000.00000000"",""owner"":""tbnb1npn682n06cly5p4yens0sxl3ezt205k3td6qfy"",""mintable"":true},{""name"":""Binance Niubility"",""symbol"":""BNN-86D"",""original_symbol"":""BNN"",""total_supply"":""100000.00000000"",""owner"":""tbnb183r6rdqk3fgm98kvkwnqjjfag0lqm6xt4a5p2g"",""mintable"":false},{""name"":""BOW not POW coin"",""symbol"":""BOW-B6F"",""original_symbol"":""BOW"",""total_supply"":""200000000.00000000"",""owner"":""tbnb1zkpj55vl8ntc849wgr2x6cw8rw5jq52e582fry"",""mintable"":false},{""name"":""Bitcoin"",""symbol"":""BTC.B-918"",""original_symbol"":""BTC.B"",""total_supply"":""21000000.00000000"",""owner"":""tbnb1fhr04azuhcj0dulm7ka40y0cqjlafwae9k9gk2"",""mintable"":true},{""name"":""Hold Tight Token"",""symbol"":""BTIGHT-262"",""original_symbol"":""BTIGHT"",""total_supply"":""1000000000.00000000"",""owner"":""tbnb1jmeeyvsekge6v5cudf66065p54tae6xa7zvaf9"",""mintable"":true},{""name"":""BitMogul Token"",""symbol"":""BTMGL-C72"",""original_symbol"":""BTMGL"",""total_supply"":""2000000000.00000000"",""owner"":""tbnb1kegegl0ydz78unf3ysmqa6y8vnxv38ulyzcfxa"",""mintable"":true},{""name"":""Better Coin"",""symbol"":""BTR-D60"",""original_symbol"":""BTR"",""total_supply"":""5000000.00000000"",""owner"":""tbnb1ceswj63vqmhyseyas5qltwa8dxjxavarswul02"",""mintable"":true},{""name"":""Basic Volatility Token"",""symbol"":""BVT-E61"",""original_symbol"":""BVT"",""total_supply"":""10000.00000000"",""owner"":""tbnb1cjzczgyj85dky4mgcwe7m5tcl8lgglmw8y3u3j"",""mintable"":true},{""name"":""Cat Girl Supporter"",""symbol"":""CAT-B07"",""original_symbol"":""CAT"",""total_supply"":""1000000000.00000000"",""owner"":""tbnb17ynkkkxh92fqn9p9upd4nd5d0u9yal8lpxqa0d"",""mintable"":false},{""name"":""Cat Girl Supporter Coin"",""symbol"":""CAT-F9B"",""original_symbol"":""CAT"",""total_supply"":""1000000000.00000000"",""owner"":""tbnb17ynkkkxh92fqn9p9upd4nd5d0u9yal8lpxqa0d"",""mintable"":false},{""name"":""iLINK CBC"",""symbol"":""CBC.B-87C"",""original_symbol"":""CBC.B"",""total_supply"":""1000000000.00000000"",""owner"":""tbnb17e3qxcgtdsve6aufaap3tg8t0ztf3r72j6j7kj"",""mintable"":true},{""name"":""CryptoBonusMiles"",""symbol"":""CBM-464"",""original_symbol"":""CBM"",""total_supply"":""5000000000.00000000"",""owner"":""tbnb1mwkj2lg8qqel80k6l5gscacyyxv3l4y6evn5yz"",""mintable"":false},{""name"":""Celer Token"",""symbol"":""CELR-42B"",""original_symbol"":""CELR"",""total_supply"":""10000000000.00000000"",""owner"":""tbnb1d68mhlhyr6y7cjlfnhl7shfawgcnqcxkag7p40"",""mintable"":false},{""name"":""Chill"",""symbol"":""CHI-BC9"",""original_symbol"":""CHI"",""total_supply"":""42000000.00000000"",""owner"":""tbnb16s7j0k5kemk07p8tse5uqa5ypntkpnk6m094fv"",""mintable"":true},{""name"":""CLIS"",""symbol"":""CLIS-EFE"",""original_symbol"":""CLIS"",""total_supply"":""10000.00000000"",""owner"":""tbnb1jlth0myl2t65gplrud27sjycehm97e0vcujazf"",""mintable"":true},{""name"":""CNN Network"",""symbol"":""CNN-210"",""original_symbol"":""CNN"",""total_supply"":""100000000.00000000"",""owner"":""tbnb16kjh42czw3fq7c9yajaj0axhvpss2tpdkgdxg3"",""mintable"":true},{""name"":""cosmos"",""symbol"":""COSMOS-587"",""original_symbol"":""COSMOS"",""total_supply"":""999999999.00000000"",""owner"":""tbnb1sylyjw032eajr9cyllp26n04300qzzre38qyv5"",""mintable"":true},{""name"":""BEST CR7"",""symbol"":""CR7-4CC"",""original_symbol"":""CR7"",""total_supply"":""1000000000.00000000"",""owner"":""tbnb1p7dnxxn9gh5v67ud9d56fjcplumcr4hz83503g"",""mintable"":true},{""name"":""CRYPRICE"",""symbol"":""CRYPRICE-150"",""original_symbol"":""CRYPRICE"",""total_supply"":""1000000000.00000000"",""owner"":""tbnb13l27qcx633auejz33lvzcpeqm2d7rsghnxknrl"",""mintable"":true},{""name"":""Crazy Alex coin"",""symbol"":""CRZYCION-751"",""original_symbol"":""CRZYCION"",""total_supply"":""0.01000000"",""owner"":""tbnb1ey04u74fl9z96faylpr54d9krcd777dsuzh77p"",""mintable"":true},{""name"":""Crazy Alex coin"",""symbol"":""CRZYCOIN-C7B"",""original_symbol"":""CRZYCOIN"",""total_supply"":""1000000000.00000000"",""owner"":""tbnb1ey04u74fl9z96faylpr54d9krcd777dsuzh77p"",""mintable"":true},{""name"":""CZ Coin"",""symbol"":""CZC-D63"",""original_symbol"":""CZC"",""total_supply"":""200000000.00000000"",""owner"":""tbnb1xdyhksetvahdgwl4k09pkcynzaul4nwly2kgmd"",""mintable"":false},{""name"":""Changpeng Zhao Binance CEO Coin"",""symbol"":""CZCOIN-33B"",""original_symbol"":""CZCOIN"",""total_supply"":""90000000000.00000000"",""owner"":""tbnb1u37ysv75tv8ehjw700nwnu263edde5nrw0l548"",""mintable"":false},{""name"":""“CZisTheBest”"",""symbol"":""CZZ-696"",""original_symbol"":""CZZ"",""total_supply"":""1000000000.00000000"",""owner"":""tbnb1ejfew3fqula4cj9tux7kzm9mc7we8qfu94ess5"",""mintable"":true},{""name"":""new dad"",""symbol"":""DAD-7FA"",""original_symbol"":""DAD"",""total_supply"":""10000000000.00000000"",""owner"":""tbnb1v87utrghmsg7ml85600mmerxjjzzmg4wjnfy89"",""mintable"":false},{""name"":""DarkArmy"",""symbol"":""DARK-EFF"",""original_symbol"":""DARK"",""total_supply"":""100000000.00000000"",""owner"":""tbnb10cnuf49f9gde0davxmna9w5kqrlzteland2uah"",""mintable"":true},{""name"":""Dawn"",""symbol"":""DAWN-676"",""original_symbol"":""DAWN"",""total_supply"":""80000000.00000000"",""owner"":""tbnb1q3mgtmczg6cxn3wlaetk2ldzt33zlapk0plncu"",""mintable"":true},{""name"":""Doge Coin"",""symbol"":""DC1-3DC"",""original_symbol"":""DC1"",""total_supply"":""10000.00000000"",""owner"":""tbnb1w0ffaqz34rkurad9rth5hnkjxju3hajmtymqej"",""mintable"":true},{""name"":""Doge Coin"",""symbol"":""DC1-4B8"",""original_symbol"":""DC1"",""total_supply"":""10000.00000000"",""owner"":""tbnb1w0ffaqz34rkurad9rth5hnkjxju3hajmtymqej"",""mintable"":true},{""name"":""DCC Network"",""symbol"":""DCC-52A"",""original_symbol"":""DCC"",""total_supply"":""100000000.00000000"",""owner"":""tbnb16kjh42czw3fq7c9yajaj0axhvpss2tpdkgdxg3"",""mintable"":true},{""name"":""DEX Coin"",""symbol"":""DEC-237"",""original_symbol"":""DEC"",""total_supply"":""200000000.00000000"",""owner"":""tbnb19m9gpu49kgeprgsdpkayxh03cgpf89hyp0thum"",""mintable"":false},{""name"":""dex coin"",""symbol"":""DEX.B-C72"",""original_symbol"":""DEX.B"",""total_supply"":""1000000000.00000000"",""owner"":""tbnb1r4gc5ftrkr9ez2khph4h5xxd0mf0hd75jf06gw"",""mintable"":true}]" }) ;
            //this.mockHttp.Setup(x=>x.GetAsync("https://testnet-dex.binance.org/api/v1/account/tbnb1zllq4cyrztmpxt7h4k92f8gpqkgmzwqpt82rek/sequence")).ReturnsAsync(new HttpResponse{Response = @"" }) ;
            //this.mockHttp.Setup(x=>x.GetAsync("https://testnet-dex.binance.org/api/v1/account/tbnb1zllq4cyrztmpxt7h4k92f8gpqkgmzwqpt82rek/sequence")).ReturnsAsync(new HttpResponse{Response = @"" }) ;

            



            return new BinanceDexApi(this.mockHttp.Object, "https://testnet-dex.binance.org/api/v1/", "tbnb");
        }

        [Test]
        public async Task GetTimeAsync_SendingRequest_ReturnsTime()
        {
            // Arrange
            BinanceDexApi unitUnderTest = this.CreateBinanceDexApi();
            DateTime apTime = DateTime.Parse("2019-04-24T10:28:27Z").ToUniversalTime();
            string blockTime = "2019-04-24T10:28:26Z";
            int code = 0;
            string message = null;

            // Act
            Times result = await unitUnderTest.GetTimeAsync();

            // Assert
            Assert.That(result.ApTime == apTime);
            Assert.That(result.BlockTime == blockTime);
            Assert.That(result.Code == code);
            Assert.That(result.Message == message);
        }

        [Test]
        public async Task GetNodeInfoAsync_SendingRequest_ReturnsNodeInfo()
        {
            // Arrange
            BinanceDexApi unitUnderTest = this.CreateBinanceDexApi();
            string json = @"{""node_info"":{""id"":""c4d94f29e765ecfe81c940e11c2e997321aa8e0f"",""listen_addr"":""10.203.43.118:27146"",""network"":""Binance-Chain-Nile"",""version"":""0.30.1"",""channels"":""3540202122233038"",""moniker"":""Zugspitze"",""other"":{""amino_version"":"""",""p2p_version"":"""",""consensus_version"":"""",""rpc_version"":"""",""tx_index"":""on"",""rpc_address"":""tcp://0.0.0.0:27147""}},""sync_info"":{""latest_block_hash"":""DC18585214F618159722EEEB51C5973800705A490A4D2EB2F48413EF2B90ED6D"",""latest_app_hash"":""39DB053E04AA1683F9ED832C4EDE65CC35D38BB44E53303ED919F1BFC0C7F08C"",""latest_block_height"":10385258,""latest_block_time"":""2019-04-24T11:02:12.664627443Z"",""catching_up"":false},""validator_info"":{""address"":""91844D296BD8E591448EFC65FD6AD51A888D58FA"",""pub_key"":[200,14,154,190,247,255,67,156,16,198,143,232,241,48,61,237,223,197,39,113,140,59,55,216,186,104,7,68,110,60,130,122],""voting_power"":100000000000}}";
            Node node = JsonConvert.DeserializeObject<Node>(json);

            // Act
            Node result = await unitUnderTest.GetNodeInfoAsync();

            // Assert
            Assert.That(node.NodeInfo.Id == result.NodeInfo.Id);
            Assert.That(node.NodeInfo.ListenAddr == result.NodeInfo.ListenAddr);
            Assert.That(node.NodeInfo.Network == result.NodeInfo.Network);
            Assert.That(node.NodeInfo.Version == result.NodeInfo.Version);
            Assert.That(node.Code == result.Code);
            Assert.That(node.Message == result.Message);
        }

        [Test]
        public async Task GetValidatorsAsync_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            BinanceDexApi unitUnderTest = this.CreateBinanceDexApi();

            // Act
            Validators result = await unitUnderTest.GetValidatorsAsync();

            // Assert
            result.ValidatorCollection.Dump();
            result.BlockHeight.Dump();
        }

        [Test]
        public async Task GetPeersAsync_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            BinanceDexApi unitUnderTest = this.CreateBinanceDexApi();

            // Act
            IEnumerable<Peer> result = await unitUnderTest.GetPeersAsync();

            // Assert
            Assert.Fail();
        }

        [Test]
        public async Task GetAccountAsync_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            BinanceDexApi unitUnderTest = this.CreateBinanceDexApi();
            string address = "";//TODO;

            // Act
            Account result = await unitUnderTest.GetAccountAsync(
                address);

            // Assert
            Assert.Fail();
        }

        [Test]
        public async Task GetAccountSequenceAsync_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            BinanceDexApi unitUnderTest = this.CreateBinanceDexApi();
            string address = "";//TODO;

            // Act
            AccountSequence result = await unitUnderTest.GetAccountSequenceAsync(
                address);

            // Assert
            Assert.Fail();
        }

        [Test]
        public async Task GetTransactionAsync_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            BinanceDexApi unitUnderTest = this.CreateBinanceDexApi();
            string transactionId = "";//TODO;

            // Act
            Transaction result = await unitUnderTest.GetTransactionAsync(
                transactionId);

            // Assert
            Assert.Fail();
        }

        [Test]
        public async Task GetTokensAsync_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            BinanceDexApi unitUnderTest = this.CreateBinanceDexApi();
            int limit = 500;
            int offset = 0;

            // Act
            IEnumerable<Token> result = await unitUnderTest.GetTokensAsync(
                limit,
                offset);

            // Assert
            Assert.Fail();
        }

        [Test]
        public async Task GetMarketPairsAsync_StateUnderTest_ExpectedBehavior()
        {
            // Arrange
            BinanceDexApi unitUnderTest = this.CreateBinanceDexApi();
            int limit = 500;
            int offset = 0;

            // Act
            MarketPairs result = await unitUnderTest.GetMarketPairsAsync(
                limit,
                offset);

            // Assert
            Assert.Fail();
        }
    }
}
