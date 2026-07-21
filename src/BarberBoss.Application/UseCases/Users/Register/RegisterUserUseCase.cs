using AutoMapper;
using BarberBoss.Communication.Requests;
using BarberBoss.Communication.Responses;

namespace BarberBoss.Application.UseCases.Users.Register;

public class RegisterUserUseCase : IRegisterUserUseCase {
    private readonly IMapper _mapper;

    public RegisterUserUseCase(Mapper mapper) {
        _mapper = mapper;
    }


    public Task<ResponseRegisteredUserJson> Execute(RequestRegisterUserJson request) {
        
    }

    private void Validate(RequestRegisterUserJson request) {

    }
}
