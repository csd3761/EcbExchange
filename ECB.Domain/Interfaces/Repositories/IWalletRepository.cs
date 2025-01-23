using ECB.Domain.Entities;

namespace ECB.Domain.Interfaces.Repositories;

public interface IWalletRepository
{
    Task<Wallet> GetByIdAsync(long id);
    Task<Wallet> CreateAsync(Wallet wallet);
    Task UpdateAsync(Wallet wallet);
}