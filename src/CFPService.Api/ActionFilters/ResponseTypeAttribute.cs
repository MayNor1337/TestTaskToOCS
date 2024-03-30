using Microsoft.AspNetCore.Mvc;

namespace CFPService.Api.ActionFilters;

internal sealed class ResponseTypeAttribute : ProducesResponseTypeAttribute
{
    public ResponseTypeAttribute(int statusCode) 
        : base(typeof(ErrorResponse), statusCode)
    {
    }
}