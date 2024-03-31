using CFPService.Domain.Models;

namespace CFPService.Domain.Validators;

public interface IApplicationDataValidator
{
    public Task Validate(ApplicationData data);
}