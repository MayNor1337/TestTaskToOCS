using CFPService.Api.Requests;
using CFPService.Api.Responses;
using Microsoft.AspNetCore.Mvc;

namespace CFPService.Api.Controllers;

[ApiController]
[Route("/application")]
public class ApplicationController : ControllerBase
{
    [HttpPost]
    public ApplicationResponse Create(CreateRequest request)
    {
        return new ApplicationResponse(Guid.NewGuid(), request.Autor, request.Activity, request.Name,
            request.Description, request.Outline);
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