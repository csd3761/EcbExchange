using ECB.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ECB.Infrastructure.Persistence;

public class EcbDbContext : DbContext
{
    public EcbDbContext(DbContextOptions<EcbDbContext> options) : base(options)
    {
    }
    
    public DbSet<Wallet> Wallets { get; set; }
}