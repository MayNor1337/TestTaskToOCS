﻿using CFPService.Domain.Entity;
using CFPService.Domain.Separated.Repositories;
using Dapper;
using Microsoft.Extensions.Options;
using Npgsql;

namespace CFPService.Infrastructure.DataAccess.Repositories;

internal sealed class UserRepository : IUserRepository
{
    private readonly IOptionsSnapshot<DataAccessOptions> _options;

    public UserRepository(IOptionsSnapshot<DataAccessOptions> options)
    {
        _options = options;
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
    
    async Task<NpgsqlConnection> GetAndOpenConnection()
    {
        var connection = new NpgsqlConnection(_options.Value.ConnectionString);
        await connection.OpenAsync();
        await connection.ReloadTypesAsync();
        return connection;
    }
}