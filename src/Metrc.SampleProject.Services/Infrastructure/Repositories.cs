using Metrc.SampleProject.Services.MemberPlanets;
using Metrc.SampleProject.Services.ShipType;
using Metrc.SampleProject.Services.ShipInfo;

namespace Metrc.SampleProject.Services.Infrastructure
{
    public static class Repositories
    {
        public static RepositoryConnectionFactory DbFactory { get; set; }

        private static MemberPlanetsRepository _MemberPlanetsRepository;
        public static MemberPlanetsRepository MemberPlanetsRepository
        {
            get
            {
                if (_MemberPlanetsRepository == null)
                {
                    _MemberPlanetsRepository = new MemberPlanetsRepository(DbFactory);
                }

                return _MemberPlanetsRepository;
            }
        }

        private static ShipTypeRepository _ShipTypeRepository;
        public static ShipTypeRepository ShipTypeRepository
        {
            get
            {
                if (_ShipTypeRepository == null)
                {
                    _ShipTypeRepository = new ShipTypeRepository(DbFactory);
                }

                return _ShipTypeRepository;
            }
        }

        private static ShipInfoRepository _ShipInfoRepository;
        public static ShipInfoRepository ShipInfoRepository
        {
            get
            {
                if (_ShipInfoRepository == null)
                {
                    _ShipInfoRepository = new ShipInfoRepository(DbFactory);
                }

                return _ShipInfoRepository;
            }
        }
    }
}
