using AutoMapper;
using BarberBoss.Communication.Responses;
using BarberBoss.Domain.Repositories.Billings;

namespace BarberBoss.Application.UseCases.Billings.GetById;

public class GetByIdBillingsUseCase : IGetByIdBillingsUseCase{
    private readonly IBillingReadOnlyRepository _repository;
    private readonly IMapper _mapper;

    public GetByIdBillingsUseCase(IBillingReadOnlyRepository repository, IMapper mapper) {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<ResponseBillingJson> Execute(long id) {
        var entity = await _repository.GetById(id);
        
        var response = _mapper.Map<ResponseBillingJson>(entity);

        return response;
    }
}
