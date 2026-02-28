using BarberBoss.Domain.Entities;
using BarberBoss.Domain.Repositories.Billings;
using Microsoft.EntityFrameworkCore;

namespace BarberBoss.Infrastructure.DataAccess.Repositories;

internal class BillingRepository : IBillingWriteOnlyRepository, IBillingReadOnlyRepository {
    private readonly BarberBossDbContext _dbContext;

    public BillingRepository(BarberBossDbContext dbContext) {
        _dbContext = dbContext;
    }

    public async Task Add(Billing billing) {
        await _dbContext.Billings.AddAsync(billing);
    }

    public async Task<List<Billing>> GetAll() {
        return await _dbContext.Billings.AsNoTracking().ToListAsync();
    }
}
