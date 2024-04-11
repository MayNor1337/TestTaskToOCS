using CFPService.Api.Requests;

namespace CFPService.Api.ValidationModels;

public record EditValidatonModel(Guid ApplicationId, EditRequest Request);