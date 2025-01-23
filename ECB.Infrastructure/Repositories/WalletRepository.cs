using ECB.Domain.Entities;
using ECB.Domain.Interfaces.Repositories;
using ECB.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace ECB.Infrastructure.Repositories;

public class WalletRepository : IWalletRepository
{
    private readonly EcbDbContext _context;

    public WalletRepository(EcbDbContext context)
    {
        _context = context;
    }

    public async Task<Wallet> GetByIdAsync(long id)
    {
        var wallet = await _context.Wallets.FirstOrDefaultAsync(w => w.Id == id);
        if (wallet == null)
        {
            throw new KeyNotFoundException($"Wallet with ID {id} was not found.");
        }
        return wallet;
    }

    public async Task<Wallet> CreateAsync(Wallet wallet)
    {
        _context.Wallets.Add(wallet);
        await _context.SaveChangesAsync();
        return wallet;
    }

    public async Task UpdateAsync(Wallet wallet)
    {
        _context.Wallets.Update(wallet);
        await _context.SaveChangesAsync();
    }
}