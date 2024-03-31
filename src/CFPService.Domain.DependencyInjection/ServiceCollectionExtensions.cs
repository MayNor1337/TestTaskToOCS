using CFPService.Domain.Models;
using CFPService.Domain.Separated.Repositories;
using CFPService.Domain.Services;
using CFPService.Domain.Services.Interfaces;
using CFPService.Domain.Validators;
using CFPService.Domain.Validators.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

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
            .AddScoped<IApplicationValidator, NewApplicationValidator>()
            .Configure<ApplicationOptions>(options =>
            {
                config.GetSection(nameof(ApplicationOptions)).Bind(options);
            })
            .AddScoped<IApplicationDataValidator, ApplicationDataValidator>(x =>
            {
                ApplicationOptions options = x.GetRequiredService<IOptionsSnapshot<ApplicationOptions>>().Value;
                return new ApplicationDataValidator(x.GetRequiredService<IActivitiesRepository>(), options);
            });
        
        return services;
    }
}