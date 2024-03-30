using System.Transactions;
using Npgsql;

namespace CFPService.Infrastructure.DataAccess.Repositories.Interfaces;

internal interface IDbRepository
{
    protected Task<NpgsqlConnection> GetAndOpenConnection();

    public TransactionScope CreateTransactionScope(
        IsolationLevel level = IsolationLevel.ReadCommitted);
}