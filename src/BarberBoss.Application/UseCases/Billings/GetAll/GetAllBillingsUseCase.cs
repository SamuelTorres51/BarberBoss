using AutoMapper;
using BarberBoss.Communication.Responses;
using BarberBoss.Domain.Repositories.Billings;
using BarberBoss.Domain.Serv_ices.LoggedUser;

namespace BarberBoss.Application.UseCases.Billings.GetAll;

public class GetAllBillingsUseCase : IGetAllBillingsUseCase {
    private readonly IBillingReadOnlyRepository _repository;
    private readonly IMapper _mapper;
    private readonly ILoggedUser _loggedUser;

    public GetAllBillingsUseCase(IBillingReadOnlyRepository repository, IMapper mapper, ILoggedUser loggedUser) {
        _repository = repository;
        _mapper = mapper;
        _loggedUser = loggedUser;
    }
    public async Task<ResponseBillingsJson> Execute() {
        var loggedUser = await _loggedUser.Get();

        var response = await _repository.GetAll(loggedUser!);
        return new ResponseBillingsJson {
            Billings = _mapper.Map<List<ResponseShortBillingJson>>(response)
        };
    }
}