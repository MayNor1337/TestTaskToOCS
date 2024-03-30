using System.ComponentModel.DataAnnotations;
using CFPService.Domain.Models;
using CFPService.Domain.Separated.Repositories;

namespace CFPService.Domain.Validators;

internal sealed class ApplicationDataValidator : IApplicationDataValidator
{
    private readonly IActivitiesRepository _activitiesRepository;

    public ApplicationDataValidator(IActivitiesRepository activitiesRepository)
    {
        _activitiesRepository = activitiesRepository;
    }

    public async Task Validate(ApplicationRequiredData data)
    {
        NullChecks(data);
        await ActivityCheck(data);
    }

    private static void NullChecks(ApplicationRequiredData data)
    {
        if (data.Author is null)
            throw new ValidationException(
                "You cannot create a request without specifying a user identifier");

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
            && data.Name.Length > 100)
        {
            throw new ValidationException("The title cannot exceed 100 characters in length");
        }

        if (data.Description is not null
            && data.Description.Length > 300)
        {
            throw new ValidationException("The description cannot exceed 300 characters in length");
        }

        if (data.Outline is not null
            && data.Outline.Length > 1000)
        {
            throw new ValidationException("The outline cannot exceed 1000 characters in length");
        }
    }

    private async Task ActivityCheck(ApplicationRequiredData data)
    {
        if(data.Activity is null)
            return;

        var activities = await _activitiesRepository.GetActivities();
        if (activities.Any(x => x.Activity == data.Activity) == false)
            throw new ValidationException("This type of activity is not provided");
    }
}