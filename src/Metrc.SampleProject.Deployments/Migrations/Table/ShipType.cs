using FluentMigrator;

namespace Metrc.SampleProject.Deployments.Migrations.Table
{
    [Migration(20191120113243)]
    public class ShipType : ForwardOnlyMigration
    {
        public override void Up()
        {
            Create.Table("ShipType")
                .WithColumn("Id").AsInt64().NotNullable().Identity()
                .WithColumn("Name").AsAnsiString(255).NotNullable()
                .WithColumn("TopSpeed").AsDouble().NotNullable()
                .WithColumn("IsArchived").AsBoolean().NotNullable();
        }
    }
}
