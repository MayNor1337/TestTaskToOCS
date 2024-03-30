using CFPService.Domain.Entity;

namespace CFPService.Domain.Services.Interfaces;

public interface IActivitiesService
{
    public IEnumerable<ActivityEntity> GetActivities();
}