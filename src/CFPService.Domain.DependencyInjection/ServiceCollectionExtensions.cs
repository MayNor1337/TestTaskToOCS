using CFPService.Domain.Models;
using CFPService.Domain.Services;
using CFPService.Domain.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CFPService.Domain.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddDomain(
        this IServiceCollection services,
        IConfiguration config)
    {
        services
            .AddScoped<IActivitiesService, ActivitiesService>()
            .AddScoped<IApplicationService, ApplicationService>()
            .AddScoped<IUserService, UserService>()
            .Configure<ApplicationOptions>(options =>
            {
                config.GetSection(nameof(ApplicationOptions)).Bind(options);
            });
        
        return services;
    }
}