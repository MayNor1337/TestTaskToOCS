using CFPService.Domain.Models;

namespace CFPService.Domain.Entity;

public record ApplicationEntity(
    Guid? Id,
    Guid? Author,
    string? Activity,
    string? Name,
    string? Description,
    string? Outline,
    DateTime? CreatedAt,
    Statuses? Status,
    DateTime? SubmittedDate
)
{
    public ApplicationEntity UpdateData(ApplicationData data)
    {
        return this with
        {
            Activity = data.Activity ?? Activity, 
            Name = data.Name ?? Name, 
            Description = data.Description ?? Description, 
            Outline = data.Outline ?? Outline
        };
    }
}