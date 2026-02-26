using AutoMapper;
using BarberBoss.Communication.Requests;
using BarberBoss.Communication.Responses;
using BarberBoss.Domain.Repositories;
using BarberBoss.Domain.Repositories.Billings;

namespace BarberBoss.Application.UseCases.Billing.Register;

public class RegisterBillingUseCase : IRegisterBillingUseCase {
    private readonly IBillingWriteOnlyRepository _repository;
    private readonly IUnityOfWork _unityOfWork;
    private readonly IMapper _mapper;

    public RegisterBillingUseCase(IBillingWriteOnlyRepository repository, IUnityOfWork unityOfWork, IMapper mapper) {
        _repository = repository;
        _unityOfWork = unityOfWork;
        _mapper = mapper;
    }


    public Task<ResponseRegistedBillingJson> Execute(RequestBillingJson request) {
        
    }

    private void Validate(RequestBillingJson request) {

    }
}
