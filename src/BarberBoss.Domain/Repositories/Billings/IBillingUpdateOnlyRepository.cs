using BarberBoss.Domain.Entities;

namespace BarberBoss.Domain.Repositories.Billings;

public interface IBillingUpdateOnlyRepository {
    void Update(Billing billing);
    Task<Billing?> GetById(Entities.User user, long id);
}
