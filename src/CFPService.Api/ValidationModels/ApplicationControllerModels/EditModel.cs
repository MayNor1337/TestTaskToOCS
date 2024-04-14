using CFPService.Api.Requests;

namespace CFPService.Api.ValidationModels.ApplicationControllerModels;

public record EditModel(Guid ApplicationId, EditRequest Request);