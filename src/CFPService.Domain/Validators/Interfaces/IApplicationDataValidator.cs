using CFPService.Domain.Models;

namespace CFPService.Domain.Validators.Interfaces;

public interface IApplicationDataValidator
{
    public Task Validate(ApplicationData data);
}