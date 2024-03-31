using CFPService.Domain.Entity;
using CFPService.Domain.Models;

namespace CFPService.Domain.Services.Interfaces;

public interface IApplicationService
{
    public Task<ApplicationEntity> CreateApplication(Guid authorId, ApplicationData applicationData);

    public Task<ApplicationEntity> EditApplication(Guid applicationId, ApplicationData newApplicationData);

    public Task DeleteApplication(Guid applicationId);

    public Task SubmitApplication(Guid applicationId);

    public Task<ApplicationEntity> GetApplication(Guid applicationId);

    public Task<IEnumerable<ApplicationEntity>> GetApplicationByDate(
        DateTime? submittedAfter,
        DateTime? unsubmittedOlder);
}