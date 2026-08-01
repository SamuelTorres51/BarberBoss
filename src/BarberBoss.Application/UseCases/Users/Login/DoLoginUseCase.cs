using BarberBoss.Communication.Requests;
using BarberBoss.Communication.Responses;
using BarberBoss.Domain.Entities;
using BarberBoss.Domain.Repositories.Users;
using BarberBoss.Domain.Security.Cryptography;
using BarberBoss.Domain.Security.Tokens;
using BarberBoss.Exception.ExceptionBase;
using Microsoft.IdentityModel.Tokens.Experimental;

namespace BarberBoss.Application.UseCases.Users.Login;

public class DoLoginUseCase : IDoLoginUseCase {
    private readonly IUserReadOnlyRepository _repository;
    private readonly IAccessTokenGeneration _accessTokenGenerator;
    private readonly IPasswordEncripter _passwordEncripter;

    public DoLoginUseCase(IUserReadOnlyRepository repository, IAccessTokenGeneration accessTokenGenerator, IPasswordEncripter passwordEncripter) {
        _repository = repository;
        _accessTokenGenerator = accessTokenGenerator;
        _passwordEncripter = passwordEncripter;
    }

    public Task<ResponseRegisteredUserJson> Execute(RequestLoginJson request) {
        var user = _repository.GetByEmail(request.Email);

        List<string> error = ["Email or password is incorrect."];

        if (user == null) {
            throw new ErrorOnValidatorException(error);
        }

        var passwordIsValid = _passwordEncripter.Verify(request.Password, user.Password);

        if(passwordIsValid == false) {
            throw new ErrorOnValidatorException(error);
        }
    }
}
