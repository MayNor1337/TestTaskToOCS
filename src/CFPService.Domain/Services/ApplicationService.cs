using CFPService.Domain.Entity;
using CFPService.Domain.Models;
using CFPService.Domain.Separated.Repositories;
using CFPService.Domain.Services.Interfaces;
using CFPService.Domain.Validators;

namespace CFPService.Domain.Services;

internal sealed class ApplicationService : IApplicationService
{
    private readonly IApplicationRepository _applicationRepository;
    private readonly IApplicationDataValidator _dataValidator;

    public ApplicationService(IApplicationRepository applicationRepository, IApplicationDataValidator dataValidator)
    {
        _applicationRepository = applicationRepository;
        _dataValidator = dataValidator;
    }

    public async Task<ApplicationEntity> CreateApplication(ApplicationRequiredData applicationData)
    {
        await _dataValidator.Validate(applicationData);

        return _applicationRepository.InsertApplication(applicationData).Result;
    }
}