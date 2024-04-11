namespace CFPService.Domain.Models;

public record ApplicationData(
    string? Activity,
    string? Name,
    string? Description,
    string? Outline)
{
    ApplicationData UpdateData(ApplicationData data)
    {
        return new ApplicationData(
            data.Activity ?? Activity,
            data.Name ?? Name,
            data.Description ?? Description,
            data.Outline ?? Outline);
    }
}