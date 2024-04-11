﻿using CFPService.Api.ActionFilters;
using CFPService.Api.Requests;
using CFPService.Api.Responses;
using CFPService.Api.ValidationModels;
using CFPService.Domain.Models;
using CFPService.Domain.Services.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace CFPService.Api.Controllers;

[ApiController]
[Route("/application")]
[ExceptionFilter]
public class ApplicationController : ControllerBase
{
    private readonly IApplicationService _applicationService;
    private readonly IValidator<CreateRequest> _createApplicationValidator;
    private readonly IValidator<EditValidatonModel> _editApplicationValidator;
    private readonly IValidator<GetApplicationByDateRequest> _getByDateValidator;
    private readonly IValidator<DeleteValidationModel> _deleteValidator;
    private readonly IValidator<SubmitValidatonModel> _submitValidator;

    public ApplicationController(
        IApplicationService applicationService, 
        IValidator<CreateRequest> createApplicationValidator, 
        IValidator<EditValidatonModel> editApplicationValidator, 
        IValidator<GetApplicationByDateRequest> getByDateValidator, 
        IValidator<DeleteValidationModel> deleteValidator, IValidator<SubmitValidatonModel> submitValidator)
    {
        _applicationService = applicationService;
        _createApplicationValidator = createApplicationValidator;
        _editApplicationValidator = editApplicationValidator;
        _getByDateValidator = getByDateValidator;
        _deleteValidator = deleteValidator;
        _submitValidator = submitValidator;
    }

    [HttpPost]
    public async Task<ApplicationResponse> Create(CreateRequest request)
    {
        await _createApplicationValidator.ValidateAsync(request);

        var application = await _applicationService.CreateApplication(
            request.Author,
            new ApplicationData(
                request.Activity,
                request.Name,
                request.Description,
                request.Outline));
        
        return new ApplicationResponse(
            application.Id,
            application.Author,
            application.Activity,
            application.Name,
            application.Description,
            application.Outline
            );
    }

    [HttpPut("{applicationId}")]
    public async Task<ApplicationResponse> Edit(Guid applicationId, EditRequest request)
    {
        await _editApplicationValidator.ValidateAsync(new EditValidatonModel(applicationId, request));

        var applicationEntity = await _applicationService.EditApplication(
            applicationId,
            new ApplicationData(
                request.Activity,
                request.Name,
                request.Description,
                request.Outline));

        return new ApplicationResponse(
            applicationEntity.Id,
            applicationEntity.Author,
            applicationEntity.Activity,
            applicationEntity.Name,
            applicationEntity.Description,
            applicationEntity.Outline);
    }

    [HttpDelete("/application/{applicationId}")]
    public async Task Delete(Guid applicationId)
    {
        await _deleteValidator.ValidateAsync(new DeleteValidationModel(applicationId));

        await _applicationService.DeleteApplication(applicationId);
    }

    [HttpPost("{applicationId}/submit")]
    public async Task Submit(Guid applicationId)
    {
        await _submitValidator.ValidateAsync(new SubmitValidatonModel(applicationId));

        await _applicationService.SubmitApplication(applicationId);
    }

    [HttpGet]
    public async Task<IEnumerable<ApplicationResponse>> GetApplications(
        [FromQuery(Name = "submittedAfter")] DateTime? submittedAfter,
        [FromQuery(Name = "unsubmittedOlder")] DateTime? unsubmittedOlder)
    {
        await _getByDateValidator.ValidateAsync(new GetApplicationByDateRequest(submittedAfter, unsubmittedOlder));
        
        var applications = await _applicationService.GetApplicationByDate(submittedAfter, unsubmittedOlder);

        return applications.Select(x => 
            new ApplicationResponse(
                x.Id,
                x.Author,
                x.Activity,
                x.Name,
                x.Description,
                x.Outline))
            .ToArray();
    }

    [HttpGet("{applicationId}")]
    public async Task<ApplicationResponse> GetApplication(Guid applicationId)
    {
        var application = await _applicationService.GetApplication(applicationId);
        return new ApplicationResponse(
            application.Id,
            application.Author,
            application.Activity,
            application.Name,
            application.Description,
            application.Outline);
    }
}