using CFPService.Domain.Entity;

namespace CFPService.Domain.Separated.Repositories;

public interface IUserRepository
{
    Task<ApplicationEntity> GetNotSubmittedApplicationByAuthor(Guid authorId);
}