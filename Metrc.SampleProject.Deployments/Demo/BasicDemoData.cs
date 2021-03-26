using FluentMigrator;
using Metrc.SampleProject.Services.Infrastructure;

namespace Metrc.SampleProject.Deployments.Demo
{
    [Migration(20191120152234)]
    public class BasicDemoData : ForwardOnlyMigration
    {
        public override void Up()
        {
            Repositories.MemberPlanetsRepository.Create("Coruscant", 0, 0, 0);
            Repositories.MemberPlanetsRepository.Create("Endor", 4598, 2356, 14323);
            Repositories.MemberPlanetsRepository.Create("Hoth", 8208, 238745, 28487);
            Repositories.MemberPlanetsRepository.Create("Tatooine", 4598, 2356, 14323);
            Repositories.MemberPlanetsRepository.Create("Mandalore", 249234, 98632, 38752);

            Repositories.ShipTypeRepository.Create("Devestator", 6.52);
            Repositories.ShipTypeRepository.Create("Executor", 8.01);
            Repositories.ShipTypeRepository.Create("Tantive", 9.99);
        }
    }
}
