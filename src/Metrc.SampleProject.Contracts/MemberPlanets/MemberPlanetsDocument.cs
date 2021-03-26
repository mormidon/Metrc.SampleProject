using System;

namespace Metrc.SampleProject.Contracts.MemberPlanets
{
    public class MemberPlanetsDocument
    {
        public Int64 Id { get; set; }
        public String Name { get; set; }
        public Int32 Xcoordinates { get; set; }
        public Int32 Ycoordinates { get; set; }
        public Int32 Zcoordinates { get; set; }
        public Boolean IsArchived { get; set; }
    }
}
