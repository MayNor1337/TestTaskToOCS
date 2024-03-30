using CFPService.Domain.Entity;
using CFPService.Domain.Models;
using CFPService.Domain.Separated.Repositories;
using CFPService.Domain.Services.Interfaces;
using CFPService.Domain.Validators;

namespace CFPService.Domain.Services;

internal sealed class ApplicationService : IApplicationService
{
    private readonly IApplicationRepository _applicationRepository;

    public ApplicationService(IApplicationRepository applicationRepository)
    {
        _applicationRepository = applicationRepository;
    }

    public ApplicationEntity CreateApplication(ApplicationRequiredData applicationData)
    {
        var validator = new ApplicationRequiredDataValidator();
        validator.Validate(applicationData);

        return _applicationRepository.InsertApplication(applicationData).Result;
    }
}