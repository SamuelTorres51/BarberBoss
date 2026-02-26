using AutoMapper;
using BarberBoss.Communication.Requests;
using BarberBoss.Communication.Responses;
using BarberBoss.Domain.Repositories;
using BarberBoss.Domain.Repositories.Billings;
using BarberBoss.Exception.ExceptionBase;
using BarberBoss.Domain.Entities;

namespace BarberBoss.Application.UseCases.Billings.Register;

public class RegisterBillingUseCase : IRegisterBillingUseCase {
    private readonly IBillingWriteOnlyRepository _repository;
    private readonly IUnityOfWork _unityOfWork;
    private readonly IMapper _mapper;

    public RegisterBillingUseCase(IBillingWriteOnlyRepository repository, IUnityOfWork unityOfWork, IMapper mapper) {
        _repository = repository;
        _unityOfWork = unityOfWork;
        _mapper = mapper;
    }


    public async Task<ResponseRegistedBillingJson> Execute(RequestBillingJson request) {
        Validate(request);
        var entity = _mapper.Map<Billing>(request);
        await _repository.Add(entity);
        await _unityOfWork.Commit();
        var response = _mapper.Map<ResponseRegistedBillingJson>(entity);
        return response;
    }

    private void Validate(RequestBillingJson request) {
        var validator = new BillingValidator();
        var validationResult = validator.Validate(request);

        if(validationResult.IsValid == false) {
            var errorsMessage = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
            throw new ErrorOnValidatorException(errorsMessage);
        }
    }
}
