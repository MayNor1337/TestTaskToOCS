using CFPService.Domain.Models;

namespace CFPService.Api.Requests;

public record CreateRequest(
    Guid Autor,
    string Type,
    string Name,
    string Description,
    string Outline);