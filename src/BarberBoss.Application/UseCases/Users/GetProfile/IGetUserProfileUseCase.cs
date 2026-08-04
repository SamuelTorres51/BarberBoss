using BarberBoss.Communication.Responses;

namespace BarberBoss.Application.UseCases.Users.GetProfile;

public interface IGetUserProfileUseCase {
    Task<ResponseUserProfileJson> Execute();
}
