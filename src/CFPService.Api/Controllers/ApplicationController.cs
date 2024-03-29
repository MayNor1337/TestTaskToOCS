using CFPService.Api.Requests;
using CFPService.Api.Responses;
using Microsoft.AspNetCore.Mvc;

namespace CFPService.Api.Controllers;

[ApiController]
[Route("/[controller]")]
public sealed class ApplicationController : ControllerBase
{

    [HttpPost]
    public CreateResponse Create(CreateRequest request)
    {
        return new CreateResponse();
    }
}