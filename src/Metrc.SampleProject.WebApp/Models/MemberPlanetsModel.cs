using System;

namespace Metrc.SampleProject.WebApp.Models
{
    public class MemberPlanetsModel
    {
        public Boolean UpdateModal { get; set; }

        public Int64 Id { get; set; }
        public String Name { get; set; }
        public Int32 xcoordinates { get; set; }
        public Int32 ycoordinates { get; set; }
        public Int32 zcoordinates { get; set; }
    }
}
