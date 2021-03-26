using FluentMigrator;

namespace Metrc.SampleProject.Deployments.Migrations.Changes
{
    [Migration(20191119222138)]
    public sealed class AddIsArchivedToMemberPlanets_20191119222138 : ForwardOnlyMigration
    {
        public override void Up()
        {
            if (!Schema.Table("MemberPlanets").Column("IsArchived").Exists())
            {
                Alter.Table("MemberPlanets").AddColumn("IsArchived").AsBoolean().SetExistingRowsTo(false).NotNullable();
            }
        }
    }
}
