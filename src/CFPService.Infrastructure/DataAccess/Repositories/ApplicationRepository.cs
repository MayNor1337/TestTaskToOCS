using CFPService.Domain.Entity;
using CFPService.Domain.Models;
using CFPService.Domain.Separated.Repositories;
using CFPService.Domain.Separated.Results;
using Dapper;
using Microsoft.Extensions.Options;
using Npgsql;

namespace CFPService.Infrastructure.DataAccess.Repositories;

internal sealed class ApplicationRepository : IApplicationRepository
{
    private readonly IOptionsSnapshot<DataAccessOptions> _options;

    public ApplicationRepository(IOptionsSnapshot<DataAccessOptions> options)
    {
        _options = options;
    }

    public async Task<GetApplicationResult> InsertApplication(Guid authorId, ApplicationData applicationData)
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
            author = authorId,
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

        return await GetApplication(id);
    }

    public async Task<GetApplicationResult> UpdateApplication(Guid applicationId, ApplicationData applicationData)
    {
        const string sqlQuery = @"
UPDATE applications
SET activity = (SELECT activity_id FROM activities WHERE activity = @activity), 
    name = @name_value, 
    description = @description_value, 
    outline = @outline_value
WHERE applications.id = @application_id";

        var sqlQueryParams = new
        {
            activity = applicationData.Activity,
            name_value = applicationData.Name,
            description_value = applicationData.Description,
            outline_value = applicationData.Outline,
            application_id = applicationId
        };

        await using var connection = await GetAndOpenConnection();
        await connection.QueryAsync(
            new CommandDefinition(
                sqlQuery,
                sqlQueryParams));

        return await GetApplication(applicationId);
    }

    public async Task SetSentStatus(Guid applicationId)
    {
        const string sqlQuery = @"
UPDATE applications
SET status = 'sent', submitted_date = CURRENT_TIMESTAMP
WHERE id = @application_id";
        
        var sqlQueryParams = new
        {
            application_id = applicationId
        };
        
        await using var connection = await GetAndOpenConnection();
        await connection.QueryAsync(
            new CommandDefinition(
                sqlQuery,
                sqlQueryParams));
    }

    public async Task Delete(Guid applicationId)
    {
        const string sqlQuery = @"
DELETE FROM applications
WHERE id = @application_id";

        var sqlQueryParams = new
        {
            application_id = applicationId
        };
        
        await using var connection = await GetAndOpenConnection();
        await connection.QueryAsync(
            new CommandDefinition(
                sqlQuery,
                sqlQueryParams));
    }

    public async Task<GetApplicationResult> GetApplication(Guid applicationId)
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
        var applications = await connection.QueryAsync<ApplicationEntity>(
            new CommandDefinition(
                sqlQuery,
                sqlQueryParams));
        
        var applicationEntities = applications as ApplicationEntity[] ?? applications.ToArray();
        if (applicationEntities.Any() == false)
            return new GetApplicationResult.ApplicationNotFound();

        return new GetApplicationResult.ApplicationFound(applicationEntities.ToArray()[0]);
    }

    public async Task<IEnumerable<ApplicationEntity>> GetApplicationsByDateSubmittedAfterDate(DateTime submittedAfterDate)
    {
        string sqlQuery = @"
SELECT *
FROM applications_view
WHERE submitted_date > @SubmittedAfterDate";

        var sqlQueryParams = new
        {
            SubmittedAfterDate = submittedAfterDate
        };

        await using var connection = await GetAndOpenConnection();
        var applications = await connection.QueryAsync<ApplicationEntity>(sqlQuery, sqlQueryParams);
    
        return applications.ToArray();
    }

    public async Task<IEnumerable<ApplicationEntity>> GetApplicationsByDateUnsubmittedOlderDate(DateTime unsubmittedOlderDate)
    {
        string sqlQuery = @"
SELECT *
FROM applications_view
WHERE status = 'draft' AND created_at < @UnsubmittedOlderDate";

        var sqlQueryParams = new
        {
            UnsubmittedOlderDate = unsubmittedOlderDate
        };

        await using var connection = await GetAndOpenConnection();
        var applications = await connection.QueryAsync<ApplicationEntity>(sqlQuery, sqlQueryParams);
    
        return applications.ToArray();
    }
    
    private async Task<NpgsqlConnection> GetAndOpenConnection()
    {
        var connection = new NpgsqlConnection(_options.Value.ConnectionString);
        await connection.OpenAsync();
        await connection.ReloadTypesAsync();
        return connection;
    }
    
    
}