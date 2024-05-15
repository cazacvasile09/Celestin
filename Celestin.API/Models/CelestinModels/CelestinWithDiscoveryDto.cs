using System;

namespace Celestin.API.Models.CelestinModels
{
    public class CelestinWithDiscoveryDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public double Mass { get; set; }

        public double EquatorialDiameter { get; set; }

        public int SurfaceTemperature { get; set; }

        public DateTime DiscoveryDate { get; set; }

        public string DiscoveryName { get; set; }

        public string ObjectType { get; set; }
    }
}
