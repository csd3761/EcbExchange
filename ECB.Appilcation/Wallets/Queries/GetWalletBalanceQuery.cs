using MediatR;

namespace ECB.Appilcation.Wallets.Queries;

public class GetWalletBalanceQuery : IRequest<decimal>
{
    public long WalletId { get; set; }
    public string TargetCurrency { get; set; }
}