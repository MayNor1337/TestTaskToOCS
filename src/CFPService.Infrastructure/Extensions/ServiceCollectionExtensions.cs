using CFPService.Infrastructure.DataAccess;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CFPService.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection service,
        IConfiguration config)
    {
        service.Configure<DataAccessOptions>(options =>
        {
            config.GetSection(nameof(DataAccessOptions)).Bind(options);
        });
        
        Postgres.MapCompositeTypes();
        
        Postgres.AddMigrations(service);

        return service;
    }
}