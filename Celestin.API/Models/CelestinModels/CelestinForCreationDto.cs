using System;
using System.ComponentModel.DataAnnotations;

namespace Celestin.API.Models.CelestinModels
{
    public class CelestinForCreationDto
    {
        [MaxLength(200)]
        public string Name { get; set; }

        public decimal Mass { get; set; }

        public decimal EquatorialDiameter { get; set; }

        public int SurfaceTemperature { get; set; }

        [Required]
        public DateTime DiscoveryDate { get; set; }

        public int DiscoverySourceId { get; set; }
    }
}
