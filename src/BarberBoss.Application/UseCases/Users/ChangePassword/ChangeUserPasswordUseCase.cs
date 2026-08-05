using BarberBoss.Application.UseCases.Users.Update;
using BarberBoss.Communication.Requests;
using BarberBoss.Domain.Entities;
using BarberBoss.Domain.Repositories;
using BarberBoss.Domain.Repositories.Users;
using BarberBoss.Domain.Security.Cryptography;
using BarberBoss.Domain.Serv_ices.LoggedUser;
using BarberBoss.Exception;
using BarberBoss.Exception.ExceptionBase;
using FluentValidation.Results;

namespace BarberBoss.Application.UseCases.Users.ChangePassword;

public class ChangeUserPasswordUseCase : IChangeUserPasswordUseCase {
    private readonly ILoggedUser _loggedUser;
    private readonly IUserUpdateOnlyRepository _repository;
    private readonly IPasswordEncripter _passwordEncripter;
    private readonly IUnityOfWork _unityOfWork;

    public ChangeUserPasswordUseCase(IUserUpdateOnlyRepository repository, IPasswordEncripter passwordEncripter, ILoggedUser loggedUser, IUnityOfWork unityOfWork) {
        _repository = repository;
        _passwordEncripter = passwordEncripter;
        _loggedUser = loggedUser;
        _unityOfWork = unityOfWork;
    }

    public async Task Execute(RequestChangePasswordJson request) {
        var loggedUser = await _loggedUser.Get();
        Validate(request, loggedUser!);

        var user = await _repository.GetById(loggedUser!.Id);
        user.Password = _passwordEncripter.Encrypt(request.NewPassword);

        _repository.Update(user);
        await _unityOfWork.Commit();
    }

    private void Validate(RequestChangePasswordJson request, User loggedUser) {
        var result = new ChangeUserPasswordValidator().Validate(request);

        var passwordMatch = _passwordEncripter.Verify(request.CurrentPassword, loggedUser.Password);

        if (passwordMatch is false) {
            result.Errors.Add(new ValidationFailure(string.Empty, ResourceErrorMessages.PASSWORD_DIFFERENT_CURRENT));
        }

        if (result.IsValid is false) {
            var errorMessages = result.Errors.Select(e => e.ErrorMessage).ToList();
            throw new ErrorOnValidatorException(errorMessages);
        }


    }
}
