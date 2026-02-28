using BarberBoss.Infrastructure.DataAccess;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using BarberBoss.Domain.Repositories;
using BarberBoss.Infrastructure.DataAccess.Repositories;
using BarberBoss.Domain.Repositories.Billings;

namespace BarberBoss.Infrastructure;

public static class DependencyInjectionExtension {
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration) {
       AddRepositories(services);
       AddDbContext(services, configuration);
    }

    private static void AddRepositories(IServiceCollection services) {
        services.AddScoped<IBillingReadOnlyRepository, BillingRepository>();
        services.AddScoped<IBillingWriteOnlyRepository, BillingRepository>();
        services.AddScoped<IUnityOfWork, UnityOfWork>();
    }

    private static void AddDbContext(IServiceCollection services, IConfiguration configuration) {
        var connectionString = configuration.GetConnectionString("Connection");
        if(connectionString is not null) {
            services.AddDbContext<BarberBossDbContext>(options => options.UseMySQL(connectionString));  
        }
    }
}
