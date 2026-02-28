using BarberBoss.Communication.Responses;

namespace BarberBoss.Application.UseCases.Billings.GetAll;

public interface IGetAllBillingsUseCase {
    Task<ResponseBillingsJson> Execute();
}
