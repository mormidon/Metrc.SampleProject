﻿using System;

namespace Metrc.SampleProject.WebApp.Models
{
    public class ShipInfoModel
    {
        public Boolean UpdateModal { get; set; }

        public Int64 Id { get; set; }
        public String Name { get; set; }
        public Boolean Occupancy { get; set; }
        public String Status { get; set; }
    }
}
