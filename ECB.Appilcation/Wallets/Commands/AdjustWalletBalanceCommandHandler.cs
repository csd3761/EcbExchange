using ECB.Domain.Interfaces.Repositories;
using ECB.Domain.Interfaces.Services;
using ECB.Domain.Interfaces.Strategies;
using MediatR;

namespace ECB.Appilcation.Wallets.Commands;

public class AdjustWalletBalanceCommandHandler : IRequestHandler<AdjustWalletBalanceCommand, Unit>
{
    private readonly IWalletRepository _walletRepository;
    private readonly ICurrencyConversionService _currencyConversionService;
    private readonly IWalletBalanceStrategyFactory _strategyFactory;

    public AdjustWalletBalanceCommandHandler(IWalletRepository walletRepository,
        ICurrencyConversionService currencyConversionService,
        IWalletBalanceStrategyFactory strategyFactory)
    {
        _walletRepository = walletRepository;
        _currencyConversionService = currencyConversionService;
        _strategyFactory = strategyFactory;
    }
    
    public async Task<Unit> Handle(AdjustWalletBalanceCommand request, CancellationToken cancellationToken)
    {
        var wallet = await _walletRepository.GetByIdAsync(request.WalletId);
        if (wallet == null)
            throw new KeyNotFoundException($"Wallet with Id={request.WalletId} not found.");

        var amountInWalletCurrency = await _currencyConversionService.ConvertAsync(request.Amount, request.Currency, wallet.Currency);

        // Use factory to get the correct strategy
        var strategy = _strategyFactory.GetStrategy(request.Strategy);
        await strategy.AdjustBalanceAsync(wallet, amountInWalletCurrency);

        await _walletRepository.UpdateAsync(wallet);
        return Unit.Value;
    }
}