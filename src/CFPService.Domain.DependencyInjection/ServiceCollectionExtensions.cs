using CFPService.Domain.Services;
using CFPService.Domain.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace CFPService.Domain.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDomain(this IServiceCollection services)
    {
        services
            .AddScoped<IActivitiesService, ActivitiesService>()
            .AddScoped<IApplicationService, ApplicationService>();
        
        return services;
    }
}