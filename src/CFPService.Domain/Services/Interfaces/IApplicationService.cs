using CFPService.Domain.Entity;
using CFPService.Domain.Models;

namespace CFPService.Domain.Services.Interfaces;

public interface IApplicationService
{
    public ApplicationEntity CreateApplication(ApplicationRequiredData applicationData);
}