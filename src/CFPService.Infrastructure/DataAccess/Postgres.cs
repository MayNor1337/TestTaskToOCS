using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Npgsql;
using Npgsql.NameTranslation;

namespace CFPService.Infrastructure.DataAccess;

public static class Postgres
{
    private static readonly INpgsqlNameTranslator Translator = new NpgsqlSnakeCaseNameTranslator();

    public static void MapCompositeTypes()
    {
        var mapper = NpgsqlConnection.GlobalTypeMapper;
        Dapper.DefaultTypeMap.MatchNamesWithUnderscores = true;
    }

    public static void AddMigrations(IServiceCollection service)
    {
        service.AddFluentMigratorCore()
            .ConfigureRunner(rb => rb.AddPostgres()
                .WithGlobalConnectionString(s =>
                {
                    var cfg = s.GetRequiredService<IOptions<DataAccessOptions>>();
                    return cfg.Value.ConnectionString;
                })
                .ScanIn(typeof(Postgres).Assembly).For.Migrations())
            .AddLogging(lb => lb.AddFluentMigratorConsole());
    }
}