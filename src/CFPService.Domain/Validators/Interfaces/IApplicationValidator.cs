using CFPService.Domain.Models;

namespace CFPService.Domain.Validators.Interfaces;

public interface IApplicationValidator
{
    Task ValidateNewApplication(Guid authorId, ApplicationData data);

    Task ValidateData(ApplicationData data);
}