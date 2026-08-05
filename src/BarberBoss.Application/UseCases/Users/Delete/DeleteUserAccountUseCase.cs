using BarberBoss.Domain.Repositories;
using BarberBoss.Domain.Repositories.Users;
using BarberBoss.Domain.Serv_ices.LoggedUser;

namespace BarberBoss.Application.UseCases.Users.Delete;

public class DeleteUserAccountUseCase : IDeleteUserAccountUseCase {
    private readonly ILoggedUser _loggedUser;
    private readonly IUserWriteOnlyRepository _repository;
    private readonly IUnityOfWork _unityOfWork;

    public DeleteUserAccountUseCase(IUserWriteOnlyRepository repository, ILoggedUser loggedUser, IUnityOfWork unityOfWork) {
        _repository = repository;
        _loggedUser = loggedUser;
        _unityOfWork = unityOfWork;
    }
    public async Task Execute() {
        var user = await _loggedUser.Get();
        await _repository.Delete(user!);
        await _unityOfWork.Commit();
    }
}
