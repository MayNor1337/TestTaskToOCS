using CFPService.Domain.Entity;
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

    public async Task<GetApplicationResult> InsertApplication(ApplicationEntity application)
    {
        const string sqlQuery = @"
INSERT INTO applications (id, author, activity, name, description, outline, created_at, status, submitted_date)
VALUES
    (
        @applications_id, 
        @author, 
        (SELECT activity_id FROM activities WHERE activity = @activity), 
        @name_value, 
        @description_value, 
        @outline_value, 
        @created_at,
        @status::status_enum,
        @submitted_date
    );
";
        var sqlQueryParams = new
        {
            applications_id = application.Id,
            author = application.Author,
            activity = application.Activity,
            name_value = application.Name,
            description_value = application.Description,
            outline_value = application.Outline,
            created_at = application.CreatedAt,
            status = application.Status.ToString().ToLower(),
            submitted_date = application.SubmittedDate
        };

        await using var connection = await GetAndOpenConnection();
        await connection.QueryAsync(
            new CommandDefinition(
                sqlQuery,
                sqlQueryParams));

        return await GetApplication(application.Id);
    }

    public async Task<GetApplicationResult> UpdateApplication(ApplicationEntity application)
    {
        const string sqlQuery = @"
UPDATE applications
SET id = @applications_id,
    author = @author,
    activity = (SELECT activity_id FROM activities WHERE activity = @activity), 
    name = @name_value, 
    description = @description_value, 
    outline = @outline_value,
    status = @status_value::status_enum,
    created_at = @created_at,
    submitted_date = @submitted_date
WHERE applications.id = @application_id";

        var sqlQueryParams = new
        {
            applications_id = application.Id,
            author = application.Author,
            activity = application.Activity,
            name_value = application.Name,
            description_value = application.Description,
            outline_value = application.Outline,
            status_value = application.Status.ToString().ToLower(),
            application_id = application.Id,
            created_at = application.CreatedAt,
            submitted_date = application.SubmittedDate
        };

        await using var connection = await GetAndOpenConnection();
        await connection.QueryAsync(
            new CommandDefinition(
                sqlQuery,
                sqlQueryParams));

        return await GetApplication(application.Id);
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
        var application = await connection.QueryFirstOrDefaultAsync<ApplicationEntity>(
            new CommandDefinition(
                sqlQuery,
                sqlQueryParams));
        
        if (application is null)
            return new GetApplicationResult.ApplicationNotFound();

        return new GetApplicationResult.ApplicationFound(application);
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