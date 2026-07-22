using BarberBoss.Domain.Entities;
using BarberBoss.Domain.Repositories.Users;
using Microsoft.EntityFrameworkCore;

namespace BarberBoss.Infrastructure.DataAccess.Repositories;

internal class UserRepository : IUserReadOnlyRepository, IUserWriteOnlyRepository {
    private readonly BarberBossDbContext _dbContext;

    public UserRepository(BarberBossDbContext dbContext) {
        _dbContext = dbContext;
    }

    public async Task<bool> ExistActiveUserWithEmail(string email) {
        return await _dbContext.Users.AnyAsync(user => user.Email.Equals(email));
    }

    public async Task Add(User user) {
        await _dbContext.Users.AddAsync(user);
    }
}
