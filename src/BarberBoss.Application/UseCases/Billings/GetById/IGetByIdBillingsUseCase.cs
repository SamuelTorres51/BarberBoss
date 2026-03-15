using BarberBoss.Communication.Responses;

namespace BarberBoss.Application.UseCases.Billings.GetById;

public interface IGetByIdBillingsUseCase {
    Task<ResponseBillingJson> Execute(long id);
}
