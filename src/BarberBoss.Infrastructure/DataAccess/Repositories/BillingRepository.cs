using BarberBoss.Domain.Entities;
using BarberBoss.Domain.Repositories.Billings;

namespace BarberBoss.Infrastructure.DataAccess.Repositories;

internal class BillingRepository : IBillingWriteOnlyRepository {
    private readonly BarberBossDbContext _dbContext;

    public BillingRepository(BarberBossDbContext dbContext) {
        _dbContext = dbContext;
    }

    public async Task Add(Billing billing) {
        await _dbContext.Billings.AddAsync(billing);
    }
}
