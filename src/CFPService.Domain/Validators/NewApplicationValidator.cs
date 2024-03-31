using System.ComponentModel.DataAnnotations;
using CFPService.Domain.Models;
using CFPService.Domain.Separated.Repositories;
using CFPService.Domain.Validators.Interfaces;

namespace CFPService.Domain.Validators;

internal sealed class NewApplicationValidator : IApplicationValidator
{
    private readonly IApplicationDataValidator _dataValidator;
    private readonly IUserRepository _userRepository;

    public NewApplicationValidator(IUserRepository userRepository, IApplicationDataValidator dataValidator)
    {
        _userRepository = userRepository;
        _dataValidator = dataValidator;
    }

    public async Task ValidateNewApplication(Guid authorId, ApplicationData applicationData)
    {
        await _dataValidator.Validate(applicationData);

        var application = await _userRepository.GetNotSubmittedApplicationByAuthor(authorId);
        if (application is not null)
            throw new ValidationException("You have an unsent application");
    }
}