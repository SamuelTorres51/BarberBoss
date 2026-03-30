
using BarberBoss.Application.AutoMapper;
using BarberBoss.Application.UseCases.Billings.Delete;
using BarberBoss.Application.UseCases.Billings.GetAll;
using BarberBoss.Application.UseCases.Billings.GetById;
using BarberBoss.Application.UseCases.Billings.Register;
using BarberBoss.Application.UseCases.Billings.Reports.Excel;
using BarberBoss.Application.UseCases.Billings.Update;
using Microsoft.Extensions.DependencyInjection;

namespace BarberBoss.Application;

public static class DependencyInjectionExtension {
    public static void AddApplication(this IServiceCollection services) {
        AddAutoMapper(services);
        AddUseCases(services);
    }

    private static void AddAutoMapper(IServiceCollection services) {
        services.AddAutoMapper(cfg => { }, typeof(AutoMapping));
    }

    private static void AddUseCases(IServiceCollection services) {
        services.AddScoped<IRegisterBillingUseCase, RegisterBillingUseCase>();
        services.AddScoped<IGetAllBillingsUseCase, GetAllBillingsUseCase>();
        services.AddScoped<IGetByIdBillingsUseCase, GetByIdBillingsUseCase>();
        services.AddScoped<IDeleteBillingUseCase, DeleteBillingUseCase>();
        services.AddScoped<IUpdateBillingUseCase, UpdateBillingUseCase>();
        services.AddScoped<IGenerateBillingsReportsExcelUseCase, GenerateBillingsReportsExcelUseCase>();
    }
}
