using CFPService.Domain.Entity;
using CFPService.Domain.Exceptions;
using CFPService.Domain.Separated.Repositories;
using CFPService.Domain.Services.Interfaces;

namespace CFPService.Domain.Services;

internal sealed class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<ApplicationEntity> GetCurrentNotSubmittedApplication(Guid authorId)
    {
        var application = await _userRepository.GetNotSubmittedApplicationByAuthor(authorId);
        if (application is null)
            throw new SearchException("The draft of your Application was not found");
        
        return application;
    }
}