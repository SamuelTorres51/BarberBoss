using AutoMapper;
using BarberBoss.Communication.Requests;
using BarberBoss.Domain.Repositories;
using BarberBoss.Domain.Repositories.Billings;
using BarberBoss.Exception;
using BarberBoss.Exception.ExceptionBase;

namespace BarberBoss.Application.UseCases.Billings.Update;

public class UpdateBillingUseCase : IUpdateBillingUseCase{
    private readonly IBillingUpdateOnlyRepository _repository;
    private readonly IUnityOfWork _unityOfWork;
    private readonly IMapper _mapper;

    public UpdateBillingUseCase(IBillingUpdateOnlyRepository repository, IUnityOfWork unityOfWork, IMapper mapper) {
        _repository = repository;
        _mapper = mapper;
        _unityOfWork = unityOfWork;
    }

    public async Task Execute(long id, RequestBillingJson request) {
        Validate(request);
        var billing = await _repository.GetById(id);
        if (billing is null) {
            throw new NotFoundException(ResourceErrorMessages.NOT_FOUND_BILLING);
        }

        _mapper.Map(request, billing);
        _repository.Update(billing);
        await _unityOfWork.Commit();
    }

    private void Validate(RequestBillingJson request) {
        var validator = new BillingValidator();
        var validationResult = validator.Validate(request);

        if (validationResult.IsValid == false) {
            var errorsMessage = validationResult.Errors.Select(e => e.ErrorMessage).ToList();
            throw new ErrorOnValidatorException(errorsMessage);
        }
    }
}
