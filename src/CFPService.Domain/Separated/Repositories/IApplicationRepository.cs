using CFPService.Domain.Entity;
using CFPService.Domain.Models;
using CFPService.Domain.Separated.Results;

namespace CFPService.Domain.Separated.Repositories;

public interface IApplicationRepository
{
     Task<GetApplicationResult> InsertApplication(Guid applicationId, ApplicationData applicationData);

     Task<GetApplicationResult> UpdateApplication(Guid applicationId, ApplicationData applicationData);

     Task SetSentStatus(Guid applicationId);

     Task Delete(Guid applicationId);

     Task<GetApplicationResult> GetApplication(Guid applicationId);

     Task<IEnumerable<ApplicationEntity>> GetApplicationsByDateSubmittedAfterDate(
         DateTime submittedAfterDate);
     
     Task<IEnumerable<ApplicationEntity>> GetApplicationsByDateUnsubmittedOlderDate(
         DateTime unsubmittedOlderDate);
}