using System;
using System.Collections.Generic;

namespace Celestin.API.DbModels
{
    public partial class Celestin
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Mass { get; set; }
        public double EquatorialDiameter { get; set; }
        public int SurfaceTemperature { get; set; }
        public DateTime DiscoveryDate { get; set; }
        public int DiscoverySourceId { get; set; }

        public virtual DiscoverySource DiscoverySource { get; set; }
    }
}
