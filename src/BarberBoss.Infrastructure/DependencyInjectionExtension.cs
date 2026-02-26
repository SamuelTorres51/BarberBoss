using BarberBoss.Infrastructure.DataAccess;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;

namespace BarberBoss.Infrastructure;

public static class DependencyInjectionExtension {
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration) {

       AddDbContext(services, configuration);
    }

    private static void AddRepositories(IServiceCollection services) {
        // futuro
    }

    private static void AddDbContext(IServiceCollection services, IConfiguration configuration) {
        var connectionString = configuration.GetConnectionString("Connection");
        if(connectionString is not null) {
            services.AddDbContext<BarberBossDbContext>(options => options.UseMySQL(connectionString));  
        }
    }
}
