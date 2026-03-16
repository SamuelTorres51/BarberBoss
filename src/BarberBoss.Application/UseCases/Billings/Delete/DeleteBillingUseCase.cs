using BarberBoss.Domain.Repositories;
using BarberBoss.Domain.Repositories.Billings;
using BarberBoss.Exception;
using BarberBoss.Exception.ExceptionBase;

namespace BarberBoss.Application.UseCases.Billings.Delete;

public class DeleteBillingUseCase : IDeleteBillingUseCase{
    private readonly IBillingWriteOnlyRepository _repository;
    private readonly IUnityOfWork _unityOfWork;

    public DeleteBillingUseCase(IBillingWriteOnlyRepository repository, IUnityOfWork unityOfWork) {
        _repository = repository;
        _unityOfWork = unityOfWork;
    }

    public async Task Execute(long id) {
        var result = await _repository.Delete(id);
        if(result is false) {
            throw new NotFoundException(ResourceErrorMessages.NOT_FOUND_BILLING);

        }
        await _unityOfWork.Commit();
    }
}
