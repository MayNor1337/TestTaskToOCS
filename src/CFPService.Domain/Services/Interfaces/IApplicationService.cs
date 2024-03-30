using CFPService.Domain.Entity;

namespace CFPService.Domain.Services.Interfaces;

public interface IApplicationService
{
    public ActivityEntity CreateApplication();
}