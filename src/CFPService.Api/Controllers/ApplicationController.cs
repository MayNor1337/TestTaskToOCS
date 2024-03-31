﻿using CFPService.Api.ActionFilters;
using CFPService.Api.Requests;
using CFPService.Api.Responses;
using CFPService.Domain.Entity;
using CFPService.Domain.Models;
using CFPService.Domain.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CFPService.Api.Controllers;

[ApiController]
[Route("/application")]
[ExceptionFilter]
public class ApplicationController : ControllerBase
{
    private IApplicationService _applicationService;

    public ApplicationController(IApplicationService applicationService)
    {
        _applicationService = applicationService;
    }

    [HttpPost]
    public async Task<ApplicationResponse> Create(CreateRequest request)
    {
        var application = await _applicationService.CreateApplication(
            request.Autor,
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

    [HttpDelete]
    public async Task Delete(Guid applicationId)
    {
        await _applicationService.DeleteApplication(applicationId);
    }

    [HttpPost("{applicationId}/submit")]
    public void Submit(Guid applicationId)
    {

    }

    [HttpGet]
    public IEnumerable<ApplicationResponse> GetApplications(
        [FromQuery(Name = "submittedAfter")] DateTime? submittedAfter,
        [FromQuery(Name = "unsubmittedOlder")] DateTime? unsubmittedOlder)
    {
        return new ApplicationResponse[] { };
    }

    [HttpGet("{applicationId}")]
    public ApplicationResponse GetApplication(Guid applicationId)
    {
        return new ApplicationResponse(applicationId, applicationId, "", "", "", "");
    }
}