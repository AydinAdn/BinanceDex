using System.Collections.Generic;
using System.Threading.Tasks;

namespace BinanceDex.Wallet
{
    public interface IWalletProvider
    {
        IWalletProviderResult<IEnumerable<WalletInfo>> GetAllWallets();
        IWalletProviderResult<WalletInfo> FindWallet   (string walletName);
        IWalletProviderResult<WalletInfo> CreateWallet (string walletName, string password,        string confirmPassword, bool overwrite = false);
        IWalletProviderResult<WalletInfo> ImportWallet (string walletName, string password,        string confirmPassword, string seedPhrase, bool overwrite = false);
        IWalletProviderResult UpdateWallet         (string walletName, string currentPassword, string newPassword,     string confirmPassword);
        IWalletProviderResult RemoveWallet         (string walletName, string password);

        Task<IWalletProviderResult<IEnumerable<WalletInfo>>> GetAllWalletsAsync();
        Task<IWalletProviderResult<WalletInfo>> FindWalletAsync  (string walletName);
        Task<IWalletProviderResult<WalletInfo>> CreateWalletAsync(string walletName, string password,        string confirmPassword, bool overwrite = false);
        Task<IWalletProviderResult<WalletInfo>> ImportWalletAsync(string walletName, string password,        string confirmPassword, string seedPhrase, bool overwrite = false);
        Task<IWalletProviderResult> UpdateWalletAsync        (string walletName, string currentPassword, string newPassword,     string confirmPassword);
        Task<IWalletProviderResult> RemoveWalletAsync        (string walletName, string password);

        // TODO Signing stuff

    }


    public interface IWalletManager
    {
        Task<IWalletManagerResult> CreateWalletAsync(string password);
        Task<IWalletManagerResult> OpenWalletAsync(string mnemonicWords, string password);
        Task<IWalletManagerResult> OpenWalletAsync(string privateKey);
    }

    public interface IWalletManagerResult
    {
        bool IsSuccessful { get; }
        Wallet Wallet { get; }
    }

    public class WalletManagerResult : IWalletManagerResult
    {
        public bool IsSuccessful { get; internal set; }
        public Wallet Wallet    { get; internal set;}
    }

    public interface IWallet
    {

    }
}