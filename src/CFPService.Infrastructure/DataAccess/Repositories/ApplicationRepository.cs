using CFPService.Domain.Entity;
using CFPService.Domain.Models;
using CFPService.Domain.Separated.Repositories;
using Dapper;
using Microsoft.Extensions.Options;

namespace CFPService.Infrastructure.DataAccess.Repositories;

internal sealed class ApplicationRepository : BaseRepository, IApplicationRepository
{
    public ApplicationRepository(IOptionsSnapshot<DataAccessOptions> options) : base(options)
    {
    }

    public async Task<ApplicationEntity> InsertApplication(ApplicationRequiredData applicationData)
    {
        const string sqlQuery = @"
INSERT INTO applications (id, author, activity, name, description, outline, created_at, status)
VALUES
    (
        @applications_id, 
        @author, 
        (SELECT activity_id FROM activities WHERE activity = @activity), 
        @name_value, 
        @description_value, 
        @outline_value, 
        CURRENT_TIMESTAMP,
        DEFAULT
    );
";
        Guid id = Guid.NewGuid();
        var sqlQueryParams = new
        {
            applications_id = id,
            author = applicationData.Author,
            activity = applicationData.Activity,
            name_value = applicationData.Name,
            description_value = applicationData.Description,
            outline_value = applicationData.Outline
        };

        await using var connection = await GetAndOpenConnection();
        await connection.QueryAsync(
            new CommandDefinition(
                sqlQuery,
                sqlQueryParams));

        return GetApplication(id).Result;
    }

    public async Task<ApplicationEntity> GetApplication(Guid applicationId)
    {
        const string sqlQuery = @"
SELECT *
FROM applications_view
WHERE applications_view.id = @application_id";

        var sqlQueryParams = new
        {
            application_id = applicationId
        };
        
        await using var connection = await GetAndOpenConnection();
        var application = await connection.QueryAsync<ApplicationEntity>(
            new CommandDefinition(
                sqlQuery,
                sqlQueryParams));
        return application.ToArray()[0];
    }
}