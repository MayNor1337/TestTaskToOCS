using CFPService.Api.ValidationModels;
using CFPService.Api.ValidationModels.ApplicationControllerModels;
using CFPService.Domain.Entity;
using CFPService.Domain.Separated.Repositories;
using CFPService.Domain.Separated.Results;
using FluentValidation;

namespace CFPService.Api.Validators.ApplicationControllerValidators;

internal sealed class SubmitValidator : AbstractValidator<SubmitValidatonModel>
{
    public SubmitValidator(IApplicationRepository applicationRepository)
    {
        RuleFor(x => x.ApplicationId)
            .NotEmpty()
            .DependentRules(() =>
            {
                ApplicationEntity application = null;

                RuleFor(id => id.ApplicationId)
                    .Cascade(CascadeMode.Stop)
                    .MustAsync(async (id, cancellation) =>
                    {
                        GetApplicationResult applicationResult = await applicationRepository.GetApplication(id);
                        if (applicationResult is not GetApplicationResult.ApplicationFound)
                            return false;

                        application = ((GetApplicationResult.ApplicationFound)applicationResult).Application;
                        return true;
                    })
                    .WithMessage("The message not found")
                    .Must(id =>
                    {
                        return application.Status is Statuses.Draft;
                    })
                    .WithMessage("The message has already been sent")
                    .Must(id =>
                    {
                        return application.Activity is not null
                               && application.Name is not null
                               && application.Outline is not null;
                    })
                    .WithMessage("You cannot submit the request without filling in the required fields");
            });
    }
}