using CFPService.Domain.Entity;
using CFPService.Domain.Separated.Repositories;
using Dapper;
using Microsoft.Extensions.Options;
using Npgsql;

namespace CFPService.Infrastructure.DataAccess.Repositories;

internal sealed class ActivitiesRepository : IActivitiesRepository
{
    private readonly IOptionsSnapshot<DataAccessOptions> _options;

    public ActivitiesRepository(IOptionsSnapshot<DataAccessOptions> options)
    {
        _options = options;
    }


    public async Task<IEnumerable<ActivityEntity>>
        GetActivities()
    {
        const string sqlQuery = @"
SELECT activity, description 
FROM activities";
        
        await using var connection = await GetAndOpenConnection();
        var activities = await connection.QueryAsync<ActivityEntity>(
            new CommandDefinition(
                sqlQuery));

        return activities.ToArray();
    }

    async Task<NpgsqlConnection> GetAndOpenConnection()
    {
        var connection = new NpgsqlConnection(_options.Value.ConnectionString);
        await connection.OpenAsync();
        await connection.ReloadTypesAsync();
        return connection;
    }
    
}