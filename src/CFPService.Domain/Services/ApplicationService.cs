using CFPService.Domain.Entity;
using CFPService.Domain.Models;
using CFPService.Domain.Separated.Repositories;
using CFPService.Domain.Services.Interfaces;

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
        return _applicationRepository.InsertApplication(applicationData).Result;
    }
}