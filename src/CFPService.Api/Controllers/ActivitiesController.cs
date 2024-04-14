using CFPService.Api.Responses;
using CFPService.Domain.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CFPService.Api.Controllers;

[ApiController]
[Route("/activities")]
public class ActivitiesController : ControllerBase
{
    private readonly IActivitiesService _activitiesService;

    public ActivitiesController(IActivitiesService activitiesService)
    {
        _activitiesService = activitiesService;
    }

    [HttpGet]
    public IEnumerable<ActivityResponse> GetActivities()
    {
        var activities = _activitiesService.GetActivities();
        return activities.Select(x =>
            new ActivityResponse(x.Activity, x.Description)).ToArray();
    }
}