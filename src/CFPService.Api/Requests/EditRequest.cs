namespace CFPService.Api.Requests;

public record EditRequest(
    string? Activity,
    string? Name,
    string? Description,
    string? Outline);