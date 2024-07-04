using System;

namespace Celestin.API.Models.CelestinModels
{
    public class CelestinForUpdationDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public double Mass { get; set; }
        public double EquatorialDiameter { get; set; }
        public int SurfaceTemperature { get; set; }
        public DateTime DiscoveryDate { get; set; }
        public int DiscoverySourceId { get; set; }
    }
}
