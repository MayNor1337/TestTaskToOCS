using System.ComponentModel.DataAnnotations;
using CFPService.Domain.Entity;
using CFPService.Domain.Exceptions;
using CFPService.Domain.Models;
using CFPService.Domain.Separated.Repositories;
using CFPService.Domain.Separated.Results;
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

    public async Task<ApplicationEntity> CreateApplication(Guid authorId, ApplicationData applicationData)
    {
        await _dataValidator.Validate(applicationData);

        GetApplicationResult result = await _applicationRepository.InsertApplication(authorId, applicationData);

        if (result is GetApplicationResult.ApplicationFound applicationFound)
            return applicationFound.Application;

        throw new OperationError();
    }

    public async Task<ApplicationEntity> EditApplication(Guid applicationId, ApplicationData newApplicationData)
    {
        GetApplicationResult findResult = await _applicationRepository.GetApplication(applicationId);

        if (findResult is not GetApplicationResult.ApplicationFound applicationFound)
            throw new ValidationException("There is no such account");

        var currentApplication = applicationFound.Application;
        if (applicationFound.Application.Status is Statuses.Sent)
            throw new ValidationException("You cannot edit a sent message");

        ApplicationData resultApplication = ResultApplication(newApplicationData, currentApplication);

        DataNullCheck(resultApplication);

        GetApplicationResult finalResult = await _applicationRepository.UpdateApplication(applicationId, resultApplication);

        if (finalResult is GetApplicationResult.ApplicationFound applicationFoundFinal)
            return applicationFoundFinal.Application;

        throw new OperationError();
    }

    public async Task DeleteApplication(Guid applicationId)
    {
        GetApplicationResult result = await _applicationRepository.GetApplication(applicationId);

        if (result is not GetApplicationResult.ApplicationFound applicationFound)
            throw new ValidationException("There is no such account");

        if (applicationFound.Application.Status is Statuses.Sent)
            throw new ValidationException("You cannot delete a sent message");

        await _applicationRepository.Delete(applicationId);
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