namespace CFPService.Domain.Models;

public record ApplicationModel(
    Guid Author,
    string Activity,
    string Name,
    string Description,
    string Outline);