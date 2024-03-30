namespace CFPService.Infrastructure.DataAccess;

public class DataAccessOptions
{
    public string ConnectionString { get; init; } = string.Empty;
    public int ApplicationNameMaxSize { get; init; }
    public int ApplicationDescriptionMaxSize { get; init; }
    public int ApplicationOutlineMaxSize { get; init; }
}