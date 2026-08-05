using BarberBoss.Domain.Entities;

namespace BarberBoss.Domain.Repositories.Users;

public interface IUserUpdateOnlyRepository {
    void Update(User user);
    Task<User> GetById(long id);
}
