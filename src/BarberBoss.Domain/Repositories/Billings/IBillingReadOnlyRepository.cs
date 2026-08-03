using BarberBoss.Domain.Entities;

namespace BarberBoss.Domain.Repositories.Billings;

public interface IBillingReadOnlyRepository {
    Task<List<Billing>> GetAll();
    Task<Billing?> GetById(Entities.User user, long id);
    Task<List<Billing>> FilterByMonth(DateOnly date);
}
