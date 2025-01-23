using ECB.Domain.Interfaces.Strategies;

namespace ECB.Infrastructure.Strategies;

public class WalletBalanceStrategyFactory : IWalletBalanceStrategyFactory
{
    private readonly IDictionary<string, IWalletBalanceStrategy> _strategies;
    
    public WalletBalanceStrategyFactory(IEnumerable<IWalletBalanceStrategy> strategies)
    {
        _strategies = new Dictionary<string, IWalletBalanceStrategy>(StringComparer.OrdinalIgnoreCase);
        
        foreach (var strategy in strategies)
        {
            var key = strategy.GetType().Name.Replace("Strategy", "", StringComparison.OrdinalIgnoreCase);
            _strategies[key] = strategy;
        }
    }
    
    public IWalletBalanceStrategy GetStrategy(string strategyName)
    {
        if (string.IsNullOrEmpty(strategyName))
        {
            throw new ArgumentNullException(nameof(strategyName), "Strategy name cannot be null or empty.");
        }


        if (_strategies.TryGetValue(strategyName, out var strategy))
        {
            return strategy;
        }

        throw new ArgumentException($"No strategy found for name: {strategyName}", nameof(strategyName));
    }
}