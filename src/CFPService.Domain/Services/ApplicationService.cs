using CFPService.Domain.Entity;
using CFPService.Domain.Exceptions;
using CFPService.Domain.Models;
using CFPService.Domain.Separated.Repositories;
using CFPService.Domain.Separated.Results;
using CFPService.Domain.Services.Interfaces;

namespace CFPService.Domain.Services;

internal sealed class ApplicationService : IApplicationService
{
    private readonly IApplicationRepository _applicationRepository;

    public ApplicationService(IApplicationRepository applicationRepository)
    {
        _applicationRepository = applicationRepository;
    }

    public async Task<ApplicationEntity> CreateApplication(Guid authorId, ApplicationData applicationData)
    {
        var application = new ApplicationEntity(authorId, applicationData);
        GetApplicationResult result = await _applicationRepository.InsertApplication(application);

        if (result is GetApplicationResult.ApplicationFound applicationFound)
            return applicationFound.Application;

        throw new OperationException();
    }

    public async Task<ApplicationEntity> EditApplication(Guid applicationId, ApplicationData newApplicationData)
    {
        var currentApplication = await GetApplication(applicationId);
        
        var resultApplication = currentApplication.UpdateData(newApplicationData);
        
        GetApplicationResult finalResult = await _applicationRepository.UpdateApplication(resultApplication);
        
        if (finalResult is GetApplicationResult.ApplicationFound applicationFoundFinal)
            return applicationFoundFinal.Application;

        throw new OperationException();
    }

    public async Task DeleteApplication(Guid applicationId)
    {
        await _applicationRepository.Delete(applicationId);
    }

    public async Task SubmitApplication(Guid applicationId)
    {
        var application = await GetApplication(applicationId);
        application = application.SetSendStatus();
        
        await _applicationRepository.UpdateApplication(application);
    }

    public async Task<ApplicationEntity> GetApplication(Guid applicationId)
    {
        GetApplicationResult findResult = await _applicationRepository.GetApplication(applicationId);

        if (findResult is GetApplicationResult.ApplicationFound applicationFound)
            return applicationFound.Application;

        throw new OperationException();
    }

    public async Task<IEnumerable<ApplicationEntity>> GetApplicationByDate(
        DateTime? submittedAfter, 
        DateTime? unsubmittedOlder)
    {
        if (submittedAfter is not null)
        {
            return await _applicationRepository.GetApplicationsByDateSubmittedAfterDate((DateTime)submittedAfter);
        }
        
        return await _applicationRepository.GetApplicationsByDateUnsubmittedOlderDate((DateTime)unsubmittedOlder);
    }
}