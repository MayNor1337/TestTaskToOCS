using CFPService.Api.ValidationModels;
using CFPService.Domain.Separated.Repositories;
using CFPService.Domain.Separated.Results;
using FluentValidation;

namespace CFPService.Api.Validators;

internal sealed class SubmitApplicationValidator : AbstractValidator<SubmitValidatonModel>
{
    public SubmitApplicationValidator(IApplicationRepository applicationRepository)
    {
        RuleFor(x => x.ApplicationId)
            .MustAsync(async (id, cancellation) =>
            {
                GetApplicationResult application = await applicationRepository.GetApplication(id);

                return application is GetApplicationResult.ApplicationFound;
            })
            .WithMessage("The message has already been sent")
            .MustAsync(async (id, cancellation) =>
            {
                var application =
                    ((GetApplicationResult.ApplicationFound)
                        await applicationRepository.GetApplication(id))
                    .Application;

                return application.Activity is null
                       || application.Name is null
                       || application.Outline is null;
            })
            .WithMessage("You cannot submit the request without filling in the required fields");
    }
}