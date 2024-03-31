using CFPService.Domain.Entity;

namespace CFPService.Domain.Separated.Results;

public record GetApplicationResult
{
    private GetApplicationResult() { }

    public record ApplicationFound(ApplicationEntity Application) : GetApplicationResult;

    public record ApplicationNotFound : GetApplicationResult;
}