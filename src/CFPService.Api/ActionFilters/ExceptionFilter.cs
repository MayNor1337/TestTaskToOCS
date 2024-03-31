using System.ComponentModel.DataAnnotations;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CFPService.Api.ActionFilters;

internal sealed class ExceptionFilter : Attribute, IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        switch (context.Exception)
        {
            case ValidationException exception:
                HandleBadRequest(context, exception);
                return;
            default:
                HandleInternalError(context);
                return;
        }
    }
    
    private static void HandleInternalError(ExceptionContext context)
    {
        var actionResult = new JsonResult(
            new ErrorResponse
            (
                HttpStatusCode.InternalServerError,
                "An error has occurred. We are already fixing it"
            ));

        actionResult.StatusCode = (int)HttpStatusCode.InternalServerError;
        context.Result = actionResult;
    }
    
    private static void HandleBadRequest(ExceptionContext context, ValidationException exception)
    {
        var contextResult = new JsonResult(
            new ErrorResponse
            (
                HttpStatusCode.BadRequest,
                exception.Message
            ));

        contextResult.StatusCode = (int)HttpStatusCode.BadRequest;
        context.Result = contextResult;
    }
}