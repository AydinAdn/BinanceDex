using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using BinanceDex.Utilities;
using BinanceDex.Utilities.Extensions;
using BinanceDex.Wallet;
using ConsoleDump;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace BinanceDex.WalletSample
{
    static class Program
    {
        static async Task Main()
        {

            IConfigurationRoot configuration = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                                                                         .AddJsonFile("appsettings.json", true, false)
                                                                         .Build();


            ServiceProvider services = new ServiceCollection().AddLogging(builder => builder.SetMinimumLevel(LogLevel.Trace).AddFile(configuration.GetSection("Logging:File")))
                                                              .AddWalletManager(configuration)
                                                              .BuildServiceProvider();


            IWalletProvider walletProvider = services.GetService<IWalletProvider>();


            string walletName            = "TestnetDemoWallet" + Guid.NewGuid().ToString();
            string walletPassword        = "12345678";
            string walletPasswordUpdated = "1234567890";
            string walletRecoverySeed    = @"tackle cousin pet true inner exhibit cradle crazy exact segment hard smooth loan beef crystal speak usage life weekend remove come sock just elite";

            // Create a wallet
            {
                IWalletProviderResult<WalletInfo> walletProviderResult = await walletProvider.CreateWalletAsync(walletName, walletPassword, walletPassword);
                WalletInfo walletInfo = walletProviderResult.Result;
                walletInfo.Dump("Create a wallet"); // `.Dump`  >>>> Prints to console
            }

            // Update a password on a wallet
            {
                IWalletProviderResult walletResult = await walletProvider.UpdateWalletAsync(walletName, walletPassword, walletPasswordUpdated, walletPasswordUpdated);
                walletResult.Dump("Updated password on wallet");
            }

            // GetAsync all wallets
            {
                IWalletProviderResult<IEnumerable<WalletInfo>> walletProviderResult = await walletProvider.GetAllWalletsAsync();
                IEnumerable<WalletInfo> wallets = walletProviderResult.Result;
                wallets.Dump("GetAsync all wallets");
            }

            // Remove the wallet
            {
                IWalletProviderResult walletResult = await walletProvider.RemoveWalletAsync(walletName, walletPasswordUpdated);
                walletResult.Dump("Remove the wallet");
            }

            // Import wallet
            {
                IWalletProviderResult<WalletInfo> walletProviderResult = await walletProvider.ImportWalletAsync(walletName, walletPassword, walletPassword, walletRecoverySeed);
                WalletInfo walletInfo = walletProviderResult.Result;
                walletInfo.Dump("Import wallet");
            }

            // Find an individual wallet
            {
                IWalletProviderResult<WalletInfo> walletProviderResult = await walletProvider.FindWalletAsync(walletName);
                WalletInfo walletInfo = walletProviderResult.Result;
                walletInfo.Dump("Finding an individual wallet");
            }

            // Cleaning up
            {
                IWalletProviderResult walletResult = await walletProvider.RemoveWalletAsync(walletName, walletPassword);
                walletResult.Dump("Cleaned up");
            }
        }

    }
}
