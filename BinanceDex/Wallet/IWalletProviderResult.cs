namespace BinanceDex.Wallet
{
    public interface IWalletProviderResult
    {
        bool Succeeded { get; }
        string Error { get; }
    }

    public interface IWalletProviderResult<out TResult> : IWalletProviderResult 
    {
        TResult Result { get; }
    }
}