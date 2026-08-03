using BarberBoss.Domain.Repositories;
using BarberBoss.Domain.Repositories.Billings;
using BarberBoss.Domain.Serv_ices.LoggedUser;
using BarberBoss.Exception;
using BarberBoss.Exception.ExceptionBase;

namespace BarberBoss.Application.UseCases.Billings.Delete;

public class DeleteBillingUseCase : IDeleteBillingUseCase{
    private readonly IBillingReadOnlyRepository _billingReadOnly;
    private readonly IBillingWriteOnlyRepository _repository;
    private readonly IUnityOfWork _unityOfWork;
    private readonly ILoggedUser _loggedUser;

    public DeleteBillingUseCase(IBillingWriteOnlyRepository repository, IUnityOfWork unityOfWork, ILoggedUser loggedUser, IBillingReadOnlyRepository billingReadOnly) {
        _repository = repository;
        _unityOfWork = unityOfWork;
        _loggedUser = loggedUser;
        _billingReadOnly = billingReadOnly;
    }

    public async Task Execute(long id) {

        var loggedUser = await _loggedUser.Get();

        var billing = await _billingReadOnly.GetById(loggedUser!, id);

        if (billing is null) {
            throw new NotFoundException(ResourceErrorMessages.NOT_FOUND_BILLING);

        }

        await _repository.Delete(id);
        
        await _unityOfWork.Commit();
    }
}
