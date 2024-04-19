using CFPService.Api.Requests;
using CFPService.Domain.Models;
using CFPService.Domain.Separated.Repositories;
using FluentValidation;
using Microsoft.Extensions.Options;

namespace CFPService.Api.Validators.ApplicationControllerValidators;

public sealed class CreateApplicationValidator : AbstractValidator<CreateRequest>
{
    private readonly IUserRepository _userRepository;

    public CreateApplicationValidator(
        IOptionsSnapshot<ApplicationOptions>  options, 
        IUserRepository userRepository, 
        IActivitiesRepository activitiesRepository)
    {
        _userRepository = userRepository;

        RuleFor(x => new ApplicationData(
                x.Activity, 
                x.Name, 
                x.Description, 
                x.Outline))
            .SetValidator(new DataApplicationValidator(options, activitiesRepository));
        
        RuleFor(x => x.Author).NotNull();
        OnlyOneUnsubmittedApplicationRule();
    }

    private void OnlyOneUnsubmittedApplicationRule()
    {
        RuleFor(x => x.Author)
            .MustAsync(async (author, cancellation) =>
            {
                var result = await _userRepository.GetNotSubmittedApplicationByAuthor(author);
                return result is null;
            })
            .WithMessage("You have an unsent application, send it first");
    }
}