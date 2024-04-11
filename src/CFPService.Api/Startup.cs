﻿using System.Net;
using CFPService.Api.ActionFilters;
using CFPService.Api.Naming;
using CFPService.Api.Requests;
using CFPService.Api.ValidationModels;
using CFPService.Api.Validators;
using CFPService.Domain.DependencyInjection;
using CFPService.Infrastructure.Extensions;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace CFPService.Api;

public class Startup
{
    private readonly IConfiguration _configuration;

    public Startup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddInfrastructure(_configuration)
            .AddDomain(_configuration)
            .AddMvc().AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.PropertyNamingPolicy = new SnakeCaseNamingPolicy();
                })
            .AddMvcOptions(SetupAction).Services
            .AddControllers().Services
            .AddEndpointsApiExplorer()
            .AddSwaggerGen()
            .AddHttpContextAccessor();
    }

    public void Configure(
        IHostEnvironment environment,
        IApplicationBuilder app)
    {
        if (environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        
        app.UseRouting();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
            endpoints.MapDefaultControllerRoute();
        });
    }
    
    void SetupAction(MvcOptions x)
    {
        x.Filters.Add(new ExceptionFilter());
        x.Filters.Add(new ResponseTypeAttribute((int)HttpStatusCode.InternalServerError));
        x.Filters.Add(new ResponseTypeAttribute((int)HttpStatusCode.BadRequest));
        x.Filters.Add(new ProducesResponseTypeAttribute((int)HttpStatusCode.OK));
    }

    void AddValidators(IServiceCollection services)
    {
        services
            .AddScoped<IValidator<CreateRequest>, CreateApplicationValidator>()
            .AddScoped<IValidator<EditValidatonModel>, EditApplicationValidator>()
            .AddScoped<IValidator<GetApplicationByDateRequest>, GetApplicationByDateValidator>();
    }
}