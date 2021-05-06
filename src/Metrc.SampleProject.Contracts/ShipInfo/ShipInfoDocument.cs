using System;

namespace Metrc.SampleProject.Contracts.ShipInfo
{
    public class ShipInfoDocument
    {
        public Int64 Id { get; set; }
        public String Name { get; set; }
        public Boolean Occupancy { get; set; }
        public String Status { get; set; }
        public Int64 ShipTypeId { get; set; }
        public Boolean IsArchived { get; set; }
    }
}
