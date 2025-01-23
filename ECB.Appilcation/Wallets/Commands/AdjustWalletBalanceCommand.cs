using MediatR;

namespace ECB.Appilcation.Wallets.Commands;

public class AdjustWalletBalanceCommand : IRequest<Unit>
{
    public long WalletId { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; }
    public string Strategy  { get; set; }
}