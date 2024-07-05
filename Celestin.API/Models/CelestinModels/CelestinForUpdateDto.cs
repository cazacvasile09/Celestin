using System;
using System.ComponentModel.DataAnnotations;

namespace Celestin.API.Models.CelestinModels
{
    public class CelestinForUpdateDto
    {
        [MaxLength(200)]
        public string Name { get; set; }

        public double Mass { get; set; }

        public double EquatorialDiameter { get; set; }

        public int SurfaceTemperature { get; set; }

        [Required]
        public DateTime DiscoveryDate { get; set; }
    }

}
