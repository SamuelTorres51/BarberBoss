using BarberBoss.Domain.Entities;

namespace BarberBoss.Domain.Repositories.Billings;

public interface IBillingUpdateOnlyRepository {
    void Update(Billing billing);
    Task<Billing?> GetById(long id);
}
