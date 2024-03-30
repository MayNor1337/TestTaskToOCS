namespace CFPService.Api.Requests;

public record CreateRequest(
    Guid Autor,
    string Activity,
    string Name,
    string Description,
    string Outline);