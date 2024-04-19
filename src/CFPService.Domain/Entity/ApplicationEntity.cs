using CFPService.Domain.Models;

namespace CFPService.Domain.Entity;

public record ApplicationEntity(
    Guid Id,
    Guid Author,
    string? Activity,
    string? Name,
    string? Description,
    string? Outline,
    DateTime CreatedAt,
    Statuses Status,
    DateTime? SubmittedDate
)
{
    public ApplicationEntity(Guid authorId, ApplicationData applicationData) 
        : this(Guid.NewGuid(),
            authorId,
            applicationData.Activity,
            applicationData.Name,
            applicationData.Description,
            applicationData.Outline,
            DateTime.Now,
            Statuses.Draft,
            null
            ) { }
    
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

    public ApplicationEntity SetSendStatus()
    {
        return this with { Status = Statuses.Sent, SubmittedDate = DateTime.Now};
    }

    public ApplicationData GetApplicationData()
    {
        return new ApplicationData(Activity, Name, Description, Outline);
    }
}