using CFPService.Domain.Entity;

namespace CFPService.Domain.Services.Interfaces;

public interface IUserService
{
    public Task<ApplicationEntity> GetCurrentNotSubmittedApplication(Guid authorId);
}