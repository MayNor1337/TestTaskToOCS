namespace CFPService.Api.Requests;

public record CreateRequest(
    Guid Author,
    string? Activity,
    string? Name,
    string? Description,
    string? Outline);