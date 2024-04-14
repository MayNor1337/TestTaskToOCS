using CFPService.Api.ValidationModels;
using CFPService.Api.ValidationModels.ApplicationControllerModels;
using CFPService.Domain.Separated.Repositories;
using CFPService.Domain.Separated.Results;
using FluentValidation;

namespace CFPService.Api.Validators.ApplicationControllerValidators;

public sealed class GetApplicationByIdValidator : AbstractValidator<GetApplicationByIdModel>
{
    public GetApplicationByIdValidator(IApplicationRepository applicationRepository)
    {
        RuleFor(x => x.ApplicationId)
            .MustAsync(async (id, cancellation) =>
            {
                GetApplicationResult result = await applicationRepository.GetApplication(id);

                return result is GetApplicationResult.ApplicationFound;
            })
            .WithMessage("Application not found");
    }
}