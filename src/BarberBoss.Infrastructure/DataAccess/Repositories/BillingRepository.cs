using BarberBoss.Domain.Entities;
using BarberBoss.Domain.Repositories.Billings;
using Microsoft.EntityFrameworkCore;

namespace BarberBoss.Infrastructure.DataAccess.Repositories;

internal class BillingRepository : IBillingWriteOnlyRepository, IBillingReadOnlyRepository, IBillingUpdateOnlyRepository {
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

    async Task<Billing?> IBillingReadOnlyRepository.GetById(long id) {
        return await _dbContext.Billings.AsNoTracking().FirstOrDefaultAsync(b => b.Id == id);
    }

    async Task<Billing?> IBillingUpdateOnlyRepository.GetById(long id) {
        return await _dbContext.Billings.FirstOrDefaultAsync(b => b.Id == id);
    }

    public async Task<bool> Delete(long id) { 
        var billing = await _dbContext.Billings.FirstOrDefaultAsync(b => b.Id == id);
        if(billing is null) {
            return false;
        }
        _dbContext.Billings.Remove(billing);
        return true;
    }

    public void Update(Billing billing) {
        _dbContext.Billings.Update(billing);
    }
}
