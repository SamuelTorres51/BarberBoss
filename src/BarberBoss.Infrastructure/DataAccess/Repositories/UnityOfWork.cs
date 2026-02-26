using BarberBoss.Domain.Repositories;

namespace BarberBoss.Infrastructure.DataAccess.Repositories;

internal class UnityOfWork : IUnityOfWork{
    private readonly BarberBossDbContext _dbContext;
    public UnityOfWork(BarberBossDbContext dbContext) {
        _dbContext = dbContext;
    }

    public async Task Commit(){
        await _dbContext.SaveChangesAsync();
    }
}
