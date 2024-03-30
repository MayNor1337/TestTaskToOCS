using CFPService.Domain.Entity;
using CFPService.Domain.Models;

namespace CFPService.Domain.Separated.Repositories;

public interface IApplicationRepository
{
     Task<ApplicationEntity> InsertApplication(ApplicationRequiredData applicationData);
}