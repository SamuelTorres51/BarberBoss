using BarberBoss.Communication.Requests;
using BarberBoss.Communication.Responses;

namespace BarberBoss.Application.UseCases.Billing.Register;

public class RegisterBillingUseCase : IRegisterBillingUseCase {
    public Task<ResponseRegistedBillingJson> Execute(RequestBillingJson request) {
        
    }

    private void Validate(RequestBillingJson request) {

    }
}
