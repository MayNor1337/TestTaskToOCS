using CFPService.Api.ValidationModels.UsersControllerModels;
using CFPService.Domain.Entity;
using CFPService.Domain.Separated.Repositories;
using FluentValidation;

namespace CFPService.Api.Validators.UsersControllerValidators;

internal sealed class GetApplicationByUserIdValidator : AbstractValidator<GetApplicationByUserIdModel>
{
    public GetApplicationByUserIdValidator(IUserRepository userRepository)
    {
        RuleFor(x => x.UserId)
            .MustAsync(async (id, cancellation) =>
            {
                ApplicationEntity? application = await userRepository.GetNotSubmittedApplicationByAuthor(id);
                return application is not null;
            })
            .WithMessage("Current application not found");
    }
}