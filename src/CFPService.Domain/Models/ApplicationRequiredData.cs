namespace CFPService.Domain.Models;

public record ApplicationRequiredData(
    Guid Author,
    string Activity,
    string Name,
    string Description,
    string Outline);