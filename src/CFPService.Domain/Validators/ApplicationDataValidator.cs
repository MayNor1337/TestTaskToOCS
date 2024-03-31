using System.ComponentModel.DataAnnotations;
using CFPService.Domain.Models;
using CFPService.Domain.Separated.Repositories;
using CFPService.Domain.Validators.Interfaces;

namespace CFPService.Domain.Validators;

internal sealed class ApplicationDataValidator : IApplicationDataValidator
{
    private readonly IActivitiesRepository _activitiesRepository;
    private readonly int _applicationNameMaxSize;
    private readonly int _applicationDescriptionMaxSize;
    private readonly int _applicationOutlineMaxSize;

    public ApplicationDataValidator(IActivitiesRepository activitiesRepository, ApplicationOptions options)
    {
        _activitiesRepository = activitiesRepository;
        _applicationNameMaxSize = options.ApplicationOutlineMaxSize;
        _applicationDescriptionMaxSize = options.ApplicationDescriptionMaxSize;
        _applicationOutlineMaxSize = options.ApplicationNameMaxSize;
    }

    public async Task Validate(ApplicationData data)
    {
        NullChecks(data);
        await ActivityCheck(data);
    }

    private void NullChecks(ApplicationData data)
    {
        if (data.Description is null
            && data.Name is null
            && data.Activity is null
            && data.Outline is null
           )
        {
            throw new ValidationException(
                "You cannot create a request without specifying at least one additional field besides the user identifier");
        }

        if (data.Name is not null
            && data.Name.Length > _applicationNameMaxSize)
        {
            throw new ValidationException($"The title cannot exceed {_applicationNameMaxSize} characters in length");
        }

        if (data.Description is not null
            && data.Description.Length > _applicationDescriptionMaxSize)
        {
            throw new ValidationException($"The description cannot exceed {_applicationDescriptionMaxSize} characters in length");
        }

        if (data.Outline is not null
            && data.Outline.Length > _applicationOutlineMaxSize)
        {
            throw new ValidationException($"The outline cannot exceed {_applicationOutlineMaxSize} characters in length");
        }
    }

    private async Task ActivityCheck(ApplicationData data)
    {
        if(data.Activity is null)
            return;

        var activities = await _activitiesRepository.GetActivities();
        if (activities.Any(x => x.Activity == data.Activity) == false)
            throw new ValidationException("This type of activity is not provided");
    }
}