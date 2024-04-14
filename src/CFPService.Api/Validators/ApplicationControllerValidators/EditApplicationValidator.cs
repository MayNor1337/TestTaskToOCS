using CFPService.Api.ValidationModels;
using CFPService.Api.ValidationModels.ApplicationControllerModels;
using CFPService.Domain.Entity;
using CFPService.Domain.Models;
using CFPService.Domain.Separated.Repositories;
using CFPService.Domain.Separated.Results;
using FluentValidation;
using Microsoft.Extensions.Options;

namespace CFPService.Api.Validators.ApplicationControllerValidators;

public sealed class EditApplicationValidator : AbstractValidator<EditModel>
{
    private readonly IApplicationRepository _applicationRepository;

    public EditApplicationValidator(
        IApplicationRepository applicationRepository,
        IOptionsSnapshot<ApplicationOptions>  options,
        IActivitiesRepository activitiesRepository)
    {
        _applicationRepository = applicationRepository;

        RuleFor(x => x.ApplicationId)
            .Must(id => id != Guid.Empty)
            .WithMessage("ID cannot be empty");

        RuleFor(x => new ApplicationData(
                x.Request.Activity, 
                x.Request.Name, 
                x.Request.Description, 
                x.Request.Outline))
            .SetValidator(new DataApplicationValidator(options, activitiesRepository));

        AllRequeredFieldsAndEditCapabilityRule();
    }

    private void AllRequeredFieldsAndEditCapabilityRule()
    {
        RuleFor(x => x)
            .NotEmpty()
            .DependentRules(() =>
            {
                ApplicationEntity application = null;
                RuleFor(x => x)
                    .Cascade(CascadeMode.Stop)
                    .MustAsync(async (model, cancellation) =>
                    {
                        GetApplicationResult result = await _applicationRepository.GetApplication(model.ApplicationId);

                        if (result is GetApplicationResult.ApplicationFound foundResult)
                        {
                            application = foundResult.Application;
                            return true;
                        }
                        return false;
                    })
                    .WithMessage("There is no such application")
                    .MustAsync(async (_, cancellation) =>
                    {
                        return application.Status is Statuses.Draft;
                    })
                    .WithMessage("You cannot edit a sent message")
                    .MustAsync(async (fullRequest, cancellation) =>
                    {
                        var newApplicationData = fullRequest.Request;

                        return (application.Activity is not null || newApplicationData.Activity is not null)
                               && (application.Name is not null || newApplicationData.Name is not null)
                               && (application.Outline is not null || newApplicationData.Outline is not null);
                    })
                    .WithMessage("All required fields must be filled in");
            });
    }
}