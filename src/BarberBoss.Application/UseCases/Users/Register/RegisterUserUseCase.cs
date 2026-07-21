using AutoMapper;
using BarberBoss.Communication.Requests;
using BarberBoss.Communication.Responses;
using BarberBoss.Exception.ExceptionBase;

namespace BarberBoss.Application.UseCases.Users.Register;

public class RegisterUserUseCase : IRegisterUserUseCase {
    private readonly IMapper _mapper;

    public RegisterUserUseCase(Mapper mapper) {
        _mapper = mapper;
    }


    public Task<ResponseRegisteredUserJson> Execute(RequestRegisterUserJson request) {
        Validate(request);
    }

    private void Validate(RequestRegisterUserJson request) {

        var result = new RegisterUserValidator().Validate(request);

        if (result.IsValid is false) {
            var errorMessages = result.Errors.Select(e => e.ErrorMessage).ToList();
            throw new ErrorOnValidatorException(errorMessages);
        }
    }
}
