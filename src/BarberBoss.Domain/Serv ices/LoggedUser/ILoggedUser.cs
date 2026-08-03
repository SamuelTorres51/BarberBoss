using BarberBoss.Domain.Entities;

namespace BarberBoss.Domain.Serv_ices.LoggedUser;

public interface ILoggedUser {
    Task<User?> Get();
}
