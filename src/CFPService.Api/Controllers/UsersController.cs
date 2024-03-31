using CFPService.Api.Responses;
using CFPService.Domain.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CFPService.Api.Controllers;

[ApiController]
[Route("/users")]
public class UsersController : ControllerBase
{
    private IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet("{userId}/currentapplication")]
    public async Task<ApplicationResponse> GetCurrentApplication(Guid userId)
    {
        var application = await _userService.GetCurrentNotSubmittedApplication(userId);
        return new ApplicationResponse(
            application.Id,
            application.Author,
            application.Activity,
            application.Name,
            application.Description,
            application.Outline);
    }
}