using BarberBoss.Domain.Entities;

namespace BarberBoss.Domain.Security.Tokens;

public interface IAccessTokenGeneration {
    string Generate(User user);
}
