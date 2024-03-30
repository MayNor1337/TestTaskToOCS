using CFPService.Domain.Entity;
using CFPService.Domain.Separated.Repositories;
using Dapper;
using Microsoft.Extensions.Options;

namespace CFPService.Infrastructure.DataAccess.Repositories;

internal sealed class ActivitiesRepository : BaseRepository, IActivitiesRepository
{
    public ActivitiesRepository(IOptionsSnapshot<DataAccessOptions> options) : base(options)
    {
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
}