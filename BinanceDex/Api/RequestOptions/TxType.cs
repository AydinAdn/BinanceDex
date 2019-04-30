using BinanceDex.Api.Models;

namespace BinanceDex.Api.RequestOptions
{
    public enum TxType
    {

        [Descriptor("NEW_ORDER")]
        NewOrder,
        [Descriptor("ISSUE_TOKEN")]
        IssueToken,
        [Descriptor("BURN_TOKEN")]
        BurnToken,
        [Descriptor("LIST_TOKEN")]
        ListToken,
        [Descriptor("CANCEL_ORDER")]
        CancelOrder,
        [Descriptor("FREEZE_TOKEN")]
        FreezeToken,
        [Descriptor("UN_FREEZE_TOKEN")]
        UnFreezeToken,
        [Descriptor("TRANSFER")]
        Transfer,
        [Descriptor("PROPOSAL")]
        Proposal,
        [Descriptor("VOTE")]
        Vote,
        [Descriptor("MINT")]
        Mint,
        [Descriptor("DEPOSIT")]
        Deposit
    }
}