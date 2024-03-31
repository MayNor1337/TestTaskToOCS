using System.ComponentModel.DataAnnotations;
using CFPService.Domain.Entity;
using CFPService.Domain.Exceptions;
using CFPService.Domain.Models;
using CFPService.Domain.Separated.Repositories;
using CFPService.Domain.Separated.Results;
using CFPService.Domain.Services.Interfaces;
using CFPService.Domain.Validators.Interfaces;

namespace CFPService.Domain.Services;

internal sealed class ApplicationService : IApplicationService
{
    private readonly IApplicationRepository _applicationRepository;
    private readonly IApplicationValidator _validator;

    public ApplicationService(IApplicationRepository applicationRepository, IApplicationValidator validator)
    {
        _applicationRepository = applicationRepository;
        _validator = validator;
    }

    public async Task<ApplicationEntity> CreateApplication(Guid authorId, ApplicationData applicationData)
    {
        await _validator.ValidateNewApplication(authorId, applicationData);

        GetApplicationResult result = await _applicationRepository.InsertApplication(authorId, applicationData);

        if (result is GetApplicationResult.ApplicationFound applicationFound)
            return applicationFound.Application;

        throw new OperationException();
    }

    public async Task<ApplicationEntity> EditApplication(Guid applicationId, ApplicationData newApplicationData)
    {
        var currentApplication = await GetApplication(applicationId);
        if (currentApplication.Status is Statuses.Sent)
            throw new ValidationException("You cannot edit a sent message");

        ApplicationData resultApplication = ResultApplication(newApplicationData, currentApplication);

        DataNullCheck(resultApplication);
        await _validator.ValidateData(resultApplication);

        GetApplicationResult finalResult = await _applicationRepository.UpdateApplication(applicationId, resultApplication);

        if (finalResult is GetApplicationResult.ApplicationFound applicationFoundFinal)
            return applicationFoundFinal.Application;

        throw new OperationException();
    }

    public async Task DeleteApplication(Guid applicationId)
    {
        var application = await GetApplication(applicationId);
        if (application.Status is Statuses.Sent)
            throw new ValidationException("You cannot delete a sent message");

        await _applicationRepository.Delete(applicationId);
    }

    public async Task SubmitApplication(Guid applicationId)
    {
        var application = await GetApplication(applicationId);
        if (application.Status is Statuses.Sent)
            throw new ValidationException("The message has already been sent");

        if (application.Activity is null
            || application.Name is null
            || application.Outline is null)
            throw new ValidationException("You cannot submit the request without filling in the required fields");

        await _applicationRepository.SetSentStatus(applicationId);
    }

    public async Task<ApplicationEntity> GetApplication(Guid applicationId)
    {
        GetApplicationResult findResult = await _applicationRepository.GetApplication(applicationId);

        if (findResult is not GetApplicationResult.ApplicationFound applicationFound)
            throw new ValidationException("There is no such application");

        return applicationFound.Application;
    }

    public async Task<IEnumerable<ApplicationEntity>> GetApplicationByDate(
        DateTime? submittedAfter, 
        DateTime? unsubmittedOlder)
    {
        if (submittedAfter is not null && unsubmittedOlder is not null)
        {
            throw new ValidationException("You cannot receive submitted and unsolicited applications at the same time");
        }

        if (submittedAfter is not null)
        {
            return await _applicationRepository.GetApplicationsByDate(submittedAfter);
        }

        if (unsubmittedOlder is not null)
        {
            return await _applicationRepository.GetApplicationsByDate(null, unsubmittedOlder);
        }

        throw new ValidationException("Specify at least one date");
    }

    private void DataNullCheck(ApplicationData resultApplication)
    {
        if (resultApplication.Activity is null
            || resultApplication.Name is null
            || resultApplication.Description is null
            || resultApplication.Outline is null)
        {
            throw new ValidationException("Fill in all the fields");
        }
    }

    private ApplicationData ResultApplication(ApplicationData newApplicationData, ApplicationEntity currentApplication)
    {
        return new ApplicationData(
            newApplicationData.Activity ?? currentApplication.Activity,
            newApplicationData.Name ?? currentApplication.Name,
            newApplicationData.Description ?? currentApplication.Description,
            newApplicationData.Outline ?? currentApplication.Outline
            );
    }
}