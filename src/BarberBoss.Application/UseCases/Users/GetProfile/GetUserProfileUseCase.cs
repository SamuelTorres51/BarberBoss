using AutoMapper;
using BarberBoss.Communication.Responses;
using BarberBoss.Domain.Serv_ices.LoggedUser;

namespace BarberBoss.Application.UseCases.Users.GetProfile;

public class GetUserProfileUseCase : IGetUserProfileUseCase {
    private readonly ILoggedUser _loggedUser;
    private readonly IMapper _mapper;

    public GetUserProfileUseCase(ILoggedUser loggedUser, IMapper mapper) {
        _loggedUser = loggedUser;
        _mapper = mapper;
    }

    public async Task<ResponseUserProfileJson> Execute() {
        var user = await _loggedUser.Get();
        var response = _mapper.Map<ResponseUserProfileJson>(user);

        return response;
    }
}
