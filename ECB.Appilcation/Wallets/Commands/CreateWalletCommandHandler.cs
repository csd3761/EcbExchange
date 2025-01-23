using ECB.Domain.Entities;
using ECB.Domain.Interfaces.Repositories;
using ECB.Domain.Interfaces.Services;
using MediatR;

namespace ECB.Appilcation.Wallets.Commands;

public class CreateWalletCommandHandler : IRequestHandler<CreateWalletCommand, long>
{
    private readonly IWalletRepository _walletRepository;
    private readonly ISupportedCurrencyService _supportedCurrencyService;

    public CreateWalletCommandHandler(IWalletRepository walletRepository,
        ISupportedCurrencyService supportedCurrencyService)
    {
        _walletRepository = walletRepository;
        _supportedCurrencyService = supportedCurrencyService;
    }

    public async Task<long> Handle(CreateWalletCommand request, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(request.Currency))
        {
            throw new ArgumentException("Currency must be provided and cannot be empty or whitespace.", nameof(request.Currency));
        }

        if (!await _supportedCurrencyService.IsSupported(request.Currency))
        {
            throw new ArgumentException($"Currency '{request.Currency}' is not supported.", nameof(request.Currency));
        }

        if (request.InitialBalance < 0)
        {
            throw new ArgumentException("Initial balance cannot be negative.", nameof(request.InitialBalance));
        }

        var wallet = new Wallet
        {
            Balance = request.InitialBalance,
            Currency = request.Currency
        };

        var created = await _walletRepository.CreateAsync(wallet);
        return created.Id;
    }
}