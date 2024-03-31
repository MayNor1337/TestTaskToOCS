namespace CFPService.Domain.Entity;

public record ApplicationEntity(
    Guid? Id,
    Guid? Author,
    string? Activity,
    string? Name,
    string? Description,
    string? Outline,
    DateTime? CreatedAt,
    Statuses? Status
    );