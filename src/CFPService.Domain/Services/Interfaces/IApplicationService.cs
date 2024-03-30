using CFPService.Domain.Entity;
using CFPService.Domain.Models;

namespace CFPService.Domain.Services.Interfaces;

public interface IApplicationService
{
    public Task<ApplicationEntity> CreateApplication(ApplicationRequiredData applicationData);
}