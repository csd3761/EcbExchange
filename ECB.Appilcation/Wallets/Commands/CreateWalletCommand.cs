using MediatR;

namespace ECB.Appilcation.Wallets.Commands;

public class CreateWalletCommand : IRequest<long>
{
    public decimal InitialBalance { get; set; }
    public string Currency { get; set; }
}