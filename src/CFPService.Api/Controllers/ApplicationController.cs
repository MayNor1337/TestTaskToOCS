using CFPService.Api.Requests;
using CFPService.Api.Responses;
using CFPService.Domain.Models;
using CFPService.Domain.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CFPService.Api.Controllers;

[ApiController]
[Route("/application")]
public class ApplicationController : ControllerBase
{
    private IApplicationService _applicationService;

    public ApplicationController(IApplicationService applicationService)
    {
        _applicationService = applicationService;
    }

    [HttpPost]
    public ApplicationResponse Create(CreateRequest request)
    {
        var application= _applicationService.CreateApplication(
            new ApplicationRequiredData(
                request.Autor,
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

    [HttpPut]
    public ApplicationResponse Edit(Guid applicationId)
    {
        return new ApplicationResponse(applicationId, applicationId, "", "", "", "");
    }

    [HttpDelete]
    public void Delete(Guid applicationId)
    {

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