using BarberBoss.Domain.Entities;

namespace BarberBoss.Domain.Repositories.Billings;

public interface IBillingReadOnlyRepository {
    Task<List<Billing>> GetAll();
}
