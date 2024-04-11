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
        GetApplicationResult result = await _applicationRepository.InsertApplication(authorId, applicationData);

        if (result is GetApplicationResult.ApplicationFound applicationFound)
            return applicationFound.Application;

        throw new OperationException();
    }

    public async Task<ApplicationEntity> EditApplication(Guid applicationId, ApplicationData newApplicationData)
    {
        var currentApplication = await GetApplication(applicationId);
        
        var resultApplication = currentApplication.UpdateData(newApplicationData);
        
        GetApplicationResult finalResult = await _applicationRepository.UpdateApplication(applicationId, 
            new ApplicationData(
                resultApplication.Activity,
                resultApplication.Name,
                resultApplication.Description,
                resultApplication.Outline
                ));
        
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
        await _applicationRepository.SetSentStatus(applicationId);
    }

    public async Task<ApplicationEntity> GetApplication(Guid applicationId)
    {
        GetApplicationResult findResult = await _applicationRepository.GetApplication(applicationId);

        if (findResult is not GetApplicationResult.ApplicationFound applicationFound)
            throw new OperationException();

        return applicationFound.Application;
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