using CFPService.Domain.Entity;
using CFPService.Domain.Separated.Repositories;
using CFPService.Domain.Services.Interfaces;

namespace CFPService.Domain.Services;

internal sealed class ActivitiesService : IActivitiesService
{
    private IActivitiesRepository _activitiesRepository;

    public ActivitiesService(IActivitiesRepository activitiesRepository)
    {
        _activitiesRepository = activitiesRepository;
    }

    public IEnumerable<ActivityEntity> GetActivities()
    {
        return _activitiesRepository.GetActivities().Result;
    }
}