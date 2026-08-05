using BarberBoss.Domain.Entities;
using BarberBoss.Domain.Repositories.Users;
using Microsoft.EntityFrameworkCore;

namespace BarberBoss.Infrastructure.DataAccess.Repositories;

internal class UserRepository : IUserReadOnlyRepository, IUserWriteOnlyRepository, IUserUpdateOnlyRepository {
    private readonly BarberBossDbContext _dbContext;

    public UserRepository(BarberBossDbContext dbContext) {
        _dbContext = dbContext;
    }

    public async Task<bool> ExistActiveUserWithEmail(string email) {
        return await _dbContext.Users.AnyAsync(user => user.Email.Equals(email));
    }

    public async Task<User?> GetUserByEmail(string email) {
        return await _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(user => user.Email.Equals(email));
    }

    public async Task Add(User user) {
        await _dbContext.Users.AddAsync(user);
    }

    public void Update(User user) {
        _dbContext.Users.Update(user);
    }
}
