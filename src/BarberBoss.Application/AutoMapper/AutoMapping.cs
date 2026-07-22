using AutoMapper;
using BarberBoss.Communication.Requests;
using BarberBoss.Communication.Responses;
using BarberBoss.Domain.Entities;


namespace BarberBoss.Application.AutoMapper;

public class AutoMapping : Profile{
    public AutoMapping() {
        RequestToEntity();
        EntityToResponse();
    }
    
    private void RequestToEntity() {
        CreateMap<RequestBillingJson, Billing>();
        CreateMap<RequestRegisterUserJson, User>()
            .ForMember(dest => dest.Password, opt => opt.Ignore());
    }

    private void EntityToResponse() {
        CreateMap<User, ResponseRegisteredUserJson>();
        CreateMap<Billing, ResponseRegistedBillingJson>();
        CreateMap<Billing, ResponseShortBillingJson>();
        CreateMap<Billing, ResponseBillingJson>();
    }
}
