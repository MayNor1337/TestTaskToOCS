namespace CFPService.Domain.Models;

public record ApplicationInfo(
    Guid Id,
    Guid Author,
    string Activity,
    string Name,
    string Description,
    string Outline
    );