using CFPService.Domain.Models;
using CFPService.Domain.Separated.Repositories;
using FluentValidation;

namespace CFPService.Api.Validators;

public sealed class DataApplicationValidator : AbstractValidator<ApplicationData>
{
    private readonly IActivitiesRepository _activitiesRepository;

    public DataApplicationValidator(ApplicationOptions options, IActivitiesRepository activitiesRepository)
    {
        _activitiesRepository = activitiesRepository;
        var applicationNameMaxSize = options.ApplicationOutlineMaxSize;
        var applicationDescriptionMaxSize = options.ApplicationDescriptionMaxSize;
        var applicationOutlineMaxSize = options.ApplicationNameMaxSize;

        LengthRule(applicationNameMaxSize, applicationDescriptionMaxSize, applicationOutlineMaxSize);
        ActivityCheck();
        NullRule();
    }
    
    private void ActivityCheck()
    {
        RuleFor(x => x.Activity)
            .MustAsync(async (activity, cancellation) =>
            {
                var activities = (await _activitiesRepository.GetActivities()).ToArray();
                return activities.Any(activityFromDb => activity == activityFromDb.Activity);
            })
            .When(x => x.Activity != null)
            .WithMessage("This type of activity is not provided");
    }
    
    private void LengthRule(int applicationNameMaxSize, int applicationDescriptionMaxSize, int applicationOutlineMaxSize)
    {
        RuleFor(x => x.Name)
            .Must(x => x == null || x.Length <= applicationNameMaxSize)
            .When(x => x.Name != null)
            .WithMessage($"The title cannot exceed {applicationNameMaxSize} characters in length");

        RuleFor(x => x.Description)
            .Must(x => x == null || x.Length <= applicationDescriptionMaxSize)
            .When(x => x.Description != null)
            .WithMessage($"The description cannot exceed {applicationDescriptionMaxSize} characters in length");

        RuleFor(x => x.Outline)
            .Must(x => x == null || x.Length <= applicationOutlineMaxSize)
            .When(x => x.Outline != null)
            .WithMessage($"The outline cannot exceed {applicationOutlineMaxSize} characters in length");
    }
    
    private void NullRule()
    {
        RuleFor(x => x.Activity)
            .NotEmpty()
            .When(x => string.IsNullOrEmpty(x.Name) 
                       && string.IsNullOrEmpty(x.Description) 
                       && string.IsNullOrEmpty(x.Outline))
            .WithMessage("You cannot create a request without specifying at least one additional field besides the user identifier");

        RuleFor(x => x.Name)
            .NotEmpty()
            .When(x => string.IsNullOrEmpty(x.Activity) 
                       && string.IsNullOrEmpty(x.Description) 
                       && string.IsNullOrEmpty(x.Outline))
            .WithMessage("You cannot create a request without specifying at least one additional field besides the user identifier");

        RuleFor(x => x.Description)
            .NotEmpty()
            .When(x => string.IsNullOrEmpty(x.Activity) 
                       && string.IsNullOrEmpty(x.Name) 
                       && string.IsNullOrEmpty(x.Outline))
            .WithMessage("You cannot create a request without specifying at least one additional field besides the user identifier");

        RuleFor(x => x.Outline)
            .NotEmpty()
            .When(x => string.IsNullOrEmpty(x.Activity) 
                       && string.IsNullOrEmpty(x.Name) 
                       && string.IsNullOrEmpty(x.Description))
            .WithMessage("You cannot create a request without specifying at least one additional field besides the user identifier");
    }
}