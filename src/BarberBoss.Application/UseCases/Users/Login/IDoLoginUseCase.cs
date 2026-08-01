using BarberBoss.Communication.Requests;
using BarberBoss.Communication.Responses;

namespace BarberBoss.Application.UseCases.Users.Login;

public interface IDoLoginUseCase {
    Task<ResponseRegisteredUserJson> Execute(RequestLoginJson request);
}
