using ECB.Domain.Entities;

namespace ECB.Domain.Interfaces.Strategies;

public interface IWalletBalanceStrategy
{
    Task AdjustBalanceAsync(Wallet wallet, decimal amount);
}