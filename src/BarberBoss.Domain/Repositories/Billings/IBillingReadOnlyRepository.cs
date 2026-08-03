using BarberBoss.Domain.Entities;

namespace BarberBoss.Domain.Repositories.Billings;

public interface IBillingReadOnlyRepository {
    Task<List<Billing>> GetAll(User user);
    Task<Billing?> GetById(User user, long id);
    Task<List<Billing>> FilterByMonth(DateOnly date);
}
