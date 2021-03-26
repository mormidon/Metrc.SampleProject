using FluentMigrator;

namespace Metrc.SampleProject.Deployments.Migrations.Table
{
    [Migration(20191119002144)]
    public class MemberPlanets : ForwardOnlyMigration
    {
        public override void Up()
        {
            Create.Table("MemberPlanets")
                .WithColumn("Id").AsInt64().NotNullable().Identity()
                .WithColumn("Name").AsAnsiString(255).NotNullable().Unique()
                .WithColumn("Xcoordinates").AsInt64().NotNullable()
                .WithColumn("Ycoordinates").AsInt64().NotNullable()
                .WithColumn("Zcoordinates").AsInt64().NotNullable()
                .WithColumn("IsArchived").AsBoolean().NotNullable();
        }
    }
}
