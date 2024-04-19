using CFPService.Domain.Entity;
using CFPService.Domain.Separated.Results;

namespace CFPService.Domain.Separated.Repositories;

public interface IApplicationRepository
{
     Task<GetApplicationResult> InsertApplication(ApplicationEntity application);

     Task<GetApplicationResult> UpdateApplication(ApplicationEntity application);

     Task SetSentStatus(Guid applicationId);

     Task Delete(Guid applicationId);

     Task<GetApplicationResult> GetApplication(Guid applicationId);

     Task<IEnumerable<ApplicationEntity>> GetApplicationsByDateSubmittedAfterDate(
         DateTime submittedAfterDate);
     
     Task<IEnumerable<ApplicationEntity>> GetApplicationsByDateUnsubmittedOlderDate(
         DateTime unsubmittedOlderDate);
}