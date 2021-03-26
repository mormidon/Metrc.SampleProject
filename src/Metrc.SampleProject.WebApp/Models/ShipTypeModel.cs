using System;

namespace Metrc.SampleProject.WebApp.Models
{
    public class ShipTypeModel
    {
        public Boolean UpdateModal { get; set; }

        public Int64 Id { get; set; }
        public String Name { get; set; }
        public Double TopSpeed { get; set; }
    }
}
