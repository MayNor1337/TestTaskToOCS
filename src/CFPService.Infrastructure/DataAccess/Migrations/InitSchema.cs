using FluentMigrator;

namespace CFPService.Infrastructure.DataAccess.Migrations;

[Migration(1)]
public class InitSchema : Migration
{
    public override void Up()
    {
        Create.Table("statuses")
            .WithColumn("id").AsInt32().PrimaryKey().Identity()
            .WithColumn("name").AsString().NotNullable();
        
        Insert.IntoTable("statuses").Row(new { name = "draft" });
        Insert.IntoTable("statuses").Row(new { name = "sent" });
        
        Create.Table("applications")
            .WithColumn("applications_id").AsString().NotNullable().PrimaryKey()
            .WithColumn("author_id").AsString().NotNullable()
            .WithColumn("activity").AsInt32()
            .WithColumn("name").AsString()
            .WithColumn("description").AsString(int.MaxValue)
            .WithColumn("outline").AsString(int.MaxValue)
            .WithColumn("created_at").AsDateTime().WithDefaultValue(SystemMethods.CurrentUTCDateTime)
            .WithColumn("status_id").AsInt32().ForeignKey("statuses", "id");
        
        Create.Table("activitis")
            .WithColumn("activitis_id").AsInt32().NotNullable().PrimaryKey().Identity()
            .WithColumn("name").AsString().NotNullable()
            .WithColumn("description").AsString().NotNullable();
        
        Create.ForeignKey()
            .FromTable("applications").ForeignColumn("activity")
            .ToTable("activitis").PrimaryColumn("activitis_id");
    }

    public override void Down()
    {
        Delete.ForeignKey().FromTable("activitis").ForeignColumn("activitis_id").ToTable("applications").PrimaryColumn("activity");
        Delete.Table("applications");
        Delete.Table("activitis");
        Delete.Table("statuses");        
    }
}