using System.Diagnostics.CodeAnalysis;

namespace BinanceDex.Wallet
{
    #region WalletProviderResult

    public class SuccessfulWalletProviderResult : IWalletProviderResult
    {
        public bool Succeeded => true;

        public string Error => null;
    }

    public sealed class SuccessfulWalletProviderResult<TResult> : SuccessfulWalletProviderResult, IWalletProviderResult<TResult>
    {
        public SuccessfulWalletProviderResult(TResult result)
        {
            this.Result = result;
        }

        public TResult Result { get; }
    }

    public class FailedWalletProviderResult : IWalletProviderResult
    {
        public FailedWalletProviderResult(string error)
        {
            this.Error = error;
        }

        public bool Succeeded => false;

        public string Error { get; }
    }

    [SuppressMessage("ReSharper", "UnassignedGetOnlyAutoProperty", Justification = "Result is intentionally null")]
    public sealed class FailedWalletProviderResult<TResult> : FailedWalletProviderResult, IWalletProviderResult<TResult>
    {
        public FailedWalletProviderResult(string error) : base(error.Replace("ERROR: ",""))
        {
        }

        public TResult Result { get; }
    }

    #endregion
}
