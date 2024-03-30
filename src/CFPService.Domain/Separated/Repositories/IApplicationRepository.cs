using CFPService.Domain.Entity;

namespace CFPService.Domain.Separated.Repositories;

public interface IApplicationRepository
{
     ApplicationEntity InsertApplication();
}