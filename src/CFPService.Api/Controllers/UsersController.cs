using CFPService.Api.ActionFilters;
using CFPService.Api.Responses;
using CFPService.Api.ValidationModels.UsersControllerModels;
using CFPService.Domain.Services.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace CFPService.Api.Controllers;

[ApiController]
[Route("/users")]
[ExceptionFilter]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly IValidator<GetApplicationByUserIdModel> _getValidator;

    public UsersController(IUserService userService, IValidator<GetApplicationByUserIdModel> getValidator)
    {
        _userService = userService;
        _getValidator = getValidator;
    }

    [HttpGet("{userId}/currentapplication")]
    public async Task<ApplicationResponse> GetCurrentApplication(Guid userId)
    {
        await _getValidator.ValidateAndThrowAsync(new GetApplicationByUserIdModel(userId));
    
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