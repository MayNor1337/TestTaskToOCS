using CFPService.Api.ValidationModels;
using CFPService.Domain.Entity;
using CFPService.Domain.Models;
using CFPService.Domain.Separated.Repositories;
using CFPService.Domain.Separated.Results;
using FluentValidation;
using Microsoft.Extensions.Options;

namespace CFPService.Api.Validators;

public sealed class EditApplicationValidator : AbstractValidator<EditValidatonModel>
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

        EditCapabilityRule();
        AllRequeredFieldsRule();
    }

    private void AllRequeredFieldsRule()
    {
        RuleFor(x => x)
            .MustAsync(async (fullRequest, cancellation) =>
            {
                var application =
                    ((GetApplicationResult.ApplicationFound)
                        await _applicationRepository.GetApplication(fullRequest.ApplicationId))
                    .Application;

                var newApplicationData = fullRequest.Request;

                return (application.Activity is not null || newApplicationData.Activity is not null)
                       && (application.Name is not null || newApplicationData.Name is not null)
                       && (application.Outline is not null || newApplicationData.Outline is not null);
            })
            .WithMessage("All required fields must be filled in");
    }

    private void EditCapabilityRule()
    {
        RuleFor(x => x.ApplicationId)
            .MustAsync(async (applicationId, cancellation) =>
            {
                GetApplicationResult result = await _applicationRepository.GetApplication(applicationId);

                return result is GetApplicationResult.ApplicationNotFound;
            })
            .WithMessage("There is no such application")
            .MustAsync(async (applicationId, cancellation) =>
            {
                GetApplicationResult result = await _applicationRepository.GetApplication(applicationId);

                return ((GetApplicationResult.ApplicationFound)result).Application.Status is Statuses.Draft;
            })
            .WithMessage("You cannot edit a sent message");
    }
}