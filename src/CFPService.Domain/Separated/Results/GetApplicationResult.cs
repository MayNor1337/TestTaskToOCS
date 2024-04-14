using CFPService.Domain.Entity;

namespace CFPService.Domain.Separated.Results;

public record GetApplicationResult
{
    private GetApplicationResult() { }

    public sealed record ApplicationFound(ApplicationEntity Application) : GetApplicationResult;

    public sealed record ApplicationNotFound : GetApplicationResult;
}