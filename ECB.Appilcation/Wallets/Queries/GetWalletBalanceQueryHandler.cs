using ECB.Domain.Interfaces.Repositories;
using ECB.Domain.Interfaces.Services;
using MediatR;

namespace ECB.Appilcation.Wallets.Queries;

public class GetWalletBalanceQueryHandler : IRequestHandler<GetWalletBalanceQuery, decimal>
{
    private readonly IWalletRepository _walletRepository;
    private readonly ICurrencyConversionService _currencyConversionService;

    public GetWalletBalanceQueryHandler(IWalletRepository walletRepository,
        ICurrencyConversionService currencyConversionService)
    {
        _walletRepository = walletRepository;
        _currencyConversionService = currencyConversionService;
    }

    public async Task<decimal> Handle(GetWalletBalanceQuery request, CancellationToken cancellationToken)
    {
        var wallet = await _walletRepository.GetByIdAsync(request.WalletId);
        if (wallet == null)
            throw new KeyNotFoundException($"Wallet with Id={request.WalletId} not found.");

        if (string.IsNullOrEmpty(request.TargetCurrency) || request.TargetCurrency == wallet.Currency)
        {
            return wallet.Balance;
        }

        var converted = await _currencyConversionService.ConvertAsync(wallet.Balance, wallet.Currency, request.TargetCurrency);
        return converted;
    }
}