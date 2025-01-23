namespace ECB.Domain.Interfaces.Strategies;

public interface IWalletBalanceStrategyFactory
{
    IWalletBalanceStrategy GetStrategy(string strategyName);
}