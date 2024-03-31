using CFPService.Domain.Entity;
using CFPService.Domain.Separated.Repositories;
using Dapper;
using Microsoft.Extensions.Options;

namespace CFPService.Infrastructure.DataAccess.Repositories;

internal sealed class UserRepository : BaseRepository, IUserRepository
{
    public UserRepository(IOptionsSnapshot<DataAccessOptions> options) : base(options)
    {
    }
    
    public async Task<ApplicationEntity?> GetNotSubmittedApplicationByAuthor(Guid authorId)
    {
        const string sqlQuery = @"
SELECT *
FROM applications_view
WHERE author = @author_id AND status = 'draft'";
        
        var sqlQueryParams = new
        {
            author_id = authorId
        };
        
        await using var connection = await GetAndOpenConnection();
        var application = await connection.QueryAsync<ApplicationEntity>(
            new CommandDefinition(
                sqlQuery,
                sqlQueryParams));

        var applicationEntities = application as ApplicationEntity[] ?? application.ToArray();
        if (applicationEntities.Length is 0)
            return null;
        
        return applicationEntities.ToArray()[0];
    }
}