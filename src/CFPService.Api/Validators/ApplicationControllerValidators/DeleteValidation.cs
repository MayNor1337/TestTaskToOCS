using CFPService.Api.ValidationModels;
using CFPService.Api.ValidationModels.ApplicationControllerModels;
using CFPService.Domain.Entity;
using CFPService.Domain.Separated.Repositories;
using CFPService.Domain.Separated.Results;
using FluentValidation;

namespace CFPService.Api.Validators.ApplicationControllerValidators;

public sealed class DeleteValidation : AbstractValidator<DeleteValidationModel>
{
    public DeleteValidation(IApplicationRepository applicationRepository)
    {
        RuleFor(x => x.ApplicationId)
            .NotEmpty().DependentRules(() =>
            {
                ApplicationEntity application = null;
                RuleFor(x => x.ApplicationId)
                    .Cascade(CascadeMode.Stop)
                    .MustAsync(async (id, cancellation) =>
                    {
                        GetApplicationResult result = await applicationRepository.GetApplication(id);

                        if (result is GetApplicationResult.ApplicationFound foundResult)
                        {
                            application = foundResult.Application;
                            return true;
                        }

                        return false;
                    })
                    .WithMessage("You cannot delete a non-existent application")
                    .MustAsync(async (id, cancellation) =>
                    {
                        return application.Status is Statuses.Draft;
                    })
                    .WithMessage("You cannot delete a sent application");
            });
    }
}