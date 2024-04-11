namespace CFPService.Api.ValidationModels;

public record GetApplicationByDateRequest(DateTime? SubmittedAfter, DateTime? UnsubmittedOlder);