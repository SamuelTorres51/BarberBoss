using AutoMapper;
using BarberBoss.Communication.Responses;
using BarberBoss.Domain.Repositories.Billings;

namespace BarberBoss.Application.UseCases.Billings.GetAll;

public class GetAllBillingsUseCase : IGetAllBillingsUseCase {
    private readonly IBillingReadOnlyRepository _repository;
    private readonly IMapper _mapper;

    public GetAllBillingsUseCase(IBillingReadOnlyRepository repository, IMapper mapper) {
        _repository = repository;
        _mapper = mapper;
    }
    public async Task<ResponseBillingsJson> Execute() {
        var response = await _repository.GetAll();
        return new ResponseBillingsJson {
            Billings = _mapper.Map<List<ResponseShortBillingJson>>(response)
        };
    }
}