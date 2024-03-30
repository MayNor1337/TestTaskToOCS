using CFPService.Domain.Separated.Repositories;
using CFPService.Infrastructure.DataAccess;
using CFPService.Infrastructure.DataAccess.Repositories;
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

        service.AddScoped<IActivitiesRepository, ActivitiesRepository>();
        
        Postgres.MapCompositeTypes();
        
        Postgres.AddMigrations(service);

        return service;
    }
}