using AutoMapper;
using BarberBoss.Communication.Responses;
using BarberBoss.Domain.Repositories.Billings;
using BarberBoss.Domain.Serv_ices.LoggedUser;

namespace BarberBoss.Application.UseCases.Billings.GetById;

public class GetByIdBillingsUseCase : IGetByIdBillingsUseCase{
    private readonly IBillingReadOnlyRepository _repository;
    private readonly IMapper _mapper;
    private readonly ILoggedUser _loggedUser;

    public GetByIdBillingsUseCase(IBillingReadOnlyRepository repository, IMapper mapper, ILoggedUser loggedUser) {
        _repository = repository;
        _mapper = mapper;
        _loggedUser = loggedUser;
    }

    public async Task<ResponseBillingJson> Execute(long id) {
        var loggedUser = await _loggedUser.Get();
        var entity = await _repository.GetById(loggedUser!, id);
        
        var response = _mapper.Map<ResponseBillingJson>(entity);

        return response;
    }
}
