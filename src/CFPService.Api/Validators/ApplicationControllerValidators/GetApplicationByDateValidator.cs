using CFPService.Api.ValidationModels;
using CFPService.Api.ValidationModels.ApplicationControllerModels;
using FluentValidation;

namespace CFPService.Api.Validators.ApplicationControllerValidators;

internal sealed class GetApplicationByDateValidator : AbstractValidator<GetApplicationByDateRequest>
{
    public GetApplicationByDateValidator()
    {
        RuleFor(x => x)
            .Must(request => (request.SubmittedAfter is not null && request.UnsubmittedOlder is not null) == false)
            .WithMessage("You cannot receive submitted and unsolicited applications at the same time")
            .Must(request => (request.SubmittedAfter is null && request.UnsubmittedOlder is null) == false)
            .WithMessage("Specify at least one date");
    }
}