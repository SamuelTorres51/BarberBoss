using BarberBoss.Communication.Requests;

namespace BarberBoss.Application.UseCases.Users.ChangePassword;

public interface IChangeUserPasswordUseCase {
    Task Execute(RequestChangePasswordJson request);
}
