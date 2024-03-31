namespace CFPService.Api.Responses;

public record ApplicationResponse(
    Guid? Id,
    Guid? Author,
    string? Activity,
    string? Name,
    string? Description,
    string? Outline
    );