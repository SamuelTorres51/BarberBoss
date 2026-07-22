using BarberBoss.Domain.Entities;

namespace BarberBoss.Domain.Repositories.Users;

public interface IUserWriteOnlyRepository {
    Task Add(User user);
}
