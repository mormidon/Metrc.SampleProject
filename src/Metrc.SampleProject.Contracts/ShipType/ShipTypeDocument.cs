using System;

namespace Metrc.SampleProject.Contracts.ShipType
{
    public class ShipTypeDocument
    {
        public Int64 Id { get; set; }
        public String Name { get; set; }
        public Double TopSpeed { get; set; }
        public Boolean IsArchived { get; set; }
    }
}
