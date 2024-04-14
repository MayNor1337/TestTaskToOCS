namespace CFPService.Api.ValidationModels.ApplicationControllerModels;

public record GetApplicationByDateRequest(DateTime? SubmittedAfter, DateTime? UnsubmittedOlder);