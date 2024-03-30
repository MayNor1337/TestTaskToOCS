using FluentMigrator;
using Microsoft.Extensions.Options;

namespace CFPService.Infrastructure.DataAccess.Migrations;

[Migration(2)]
public class InitSchemaActual : Migration
{
    private readonly int _applicationNameMaxSize;
    private readonly int _applicationDescriptionMaxSize;
    private readonly int _applicationOutlineMaxSize;

    public InitSchemaActual(IOptionsSnapshot<DataAccessOptions> options)
    {
        _applicationNameMaxSize = options.Value.ApplicationNameMaxSize;
        _applicationDescriptionMaxSize = options.Value.ApplicationDescriptionMaxSize;
        _applicationOutlineMaxSize = options.Value.ApplicationOutlineMaxSize;
    }

    public override void Up()
    {
        Execute.Sql("CREATE TYPE status_enum AS ENUM ('draft', 'sent');");
        
        Create.Table("applications")
            .WithColumn("id").AsGuid().NotNullable().PrimaryKey()
            .WithColumn("author").AsGuid().NotNullable()
            .WithColumn("activity").AsInt32().Nullable()
            .WithColumn("name").AsString(_applicationNameMaxSize).Nullable()
            .WithColumn("description").AsString(_applicationDescriptionMaxSize).Nullable()
            .WithColumn("outline").AsString(_applicationOutlineMaxSize).Nullable()
            .WithColumn("created_at").AsDateTime().WithDefaultValue(SystemMethods.CurrentUTCDateTime)
            .WithColumn("status").AsCustom("status_enum").WithDefaultValue("draft").Nullable();
        
        Create.Table("activities")
            .WithColumn("activity_id").AsInt32().NotNullable().PrimaryKey().Identity()
            .WithColumn("activity").AsString().NotNullable()
            .WithColumn("description").AsString().NotNullable();
        
        Insert.IntoTable("activities").Row(new
        {
            activity = "Report",
            description = "Доклад, 35-45 минут"
        });

        Insert.IntoTable("activities").Row(new
        {
            activity = "Masterclass",
            description = "Мастеркласс, 1-2 часа"
        });

        Insert.IntoTable("activities").Row(new
        {
            activity = "Discussion",
            description = "Дискуссия / круглый стол, 40-50 минут"
        });
        
        Create.ForeignKey()
            .FromTable("applications").ForeignColumn("activity")
            .ToTable("activities").PrimaryColumn("activity_id");
        
        Execute.Sql(@"
            CREATE VIEW applications_view AS
            SELECT
                a.id,
                a.author,
                act.activity,
                a.name,
                a.description,
                a.outline,
                a.created_at,
                a.status
            FROM
                applications a
            JOIN
                activities act ON a.activity = act.activity_id
        ");

        Create.Index("author_index")
            .OnTable("applications")
            .OnColumn("author");
    }

    public override void Down()
    {
        Delete.ForeignKey().FromTable("activitis").ForeignColumn("activitis_id").ToTable("applications").PrimaryColumn("activity");
        Execute.Sql("DROP VIEW IF EXISTS applications_view;");
        Delete.Table("applications");
        Delete.Table("activitis");
        Execute.Sql("DROP TYPE IF EXISTS status_enum;");
    }
}