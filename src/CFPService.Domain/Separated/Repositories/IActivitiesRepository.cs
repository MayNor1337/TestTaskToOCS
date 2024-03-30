using CFPService.Domain.Entity;

namespace CFPService.Domain.Separated.Repositories;

public interface IActivitiesRepository
{
    public Task<IEnumerable<ActivityEntity>> GetActivities();
}