using ECB.Domain.Entities;
using ECB.Domain.Interfaces.Strategies;

namespace ECB.Infrastructure.Strategies;

public class SubtractFundsStrategy : IWalletBalanceStrategy
{
    public Task AdjustBalanceAsync(Wallet wallet, decimal amount)
    {
        if (wallet.Balance < amount)
            throw new InvalidOperationException("Insufficient funds");
        
        wallet.Balance -= amount;
        return Task.CompletedTask;
    }
}