using ECB.Domain.Entities;
using ECB.Domain.Interfaces.Strategies;

namespace ECB.Infrastructure.Strategies;

public class ForceSubtractFundsStrategy : IWalletBalanceStrategy
{
    public Task AdjustBalanceAsync(Wallet wallet, decimal amount)
    {
        wallet.Balance -= amount;
        return Task.CompletedTask;
    }
}