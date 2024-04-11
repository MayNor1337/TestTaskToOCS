using CFPService.Api.ValidationModels;
using CFPService.Domain.Entity;
using CFPService.Domain.Separated.Repositories;
using CFPService.Domain.Separated.Results;
using FluentValidation;

namespace CFPService.Api.Validators;

public sealed class DeleteValidation : AbstractValidator<DeleteValidationModel>
{
    public DeleteValidation(IApplicationRepository applicationRepository)
    {
        RuleFor(x => x.ApplicationId)
            .MustAsync(async (id, cancellation) =>
            {
                GetApplicationResult application = await applicationRepository.GetApplication(id);

                return application is GetApplicationResult.ApplicationFound;
            })
            .WithMessage("You cannot delete a sent message");
    }
}