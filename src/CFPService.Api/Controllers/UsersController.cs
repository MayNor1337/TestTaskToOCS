using CFPService.Api.Responses;
using Microsoft.AspNetCore.Mvc;

namespace CFPService.Api.Controllers;

[ApiController]
[Route("/users")]
public class UsersController : ControllerBase
{
    [HttpGet("{userId}/currentapplication")]
    public ApplicationResponse GetCurrentApplication(Guid userId)
    {
        return new ApplicationResponse(userId, userId, "", "", "", "");
    }
}