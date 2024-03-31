using System.ComponentModel.DataAnnotations;
using CFPService.Domain.Entity;
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
        if (authorId == Guid.Empty)
            throw new ValidationException("Specify the author's details");

        await ValidateData(applicationData);

        ApplicationEntity? application = await _userRepository.GetNotSubmittedApplicationByAuthor(authorId);
        if (application is null)
            return;

        throw new ValidationException("You have an unsent application");
    }

    public async Task ValidateData(ApplicationData data)
    {
        await _dataValidator.Validate(data);    }
}