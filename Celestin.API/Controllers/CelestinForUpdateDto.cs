using System;
using System.ComponentModel.DataAnnotations;

namespace Celestin.API.Controllers
{
    public class CelestinForUpdateDto
    {
        [MaxLength(200)]

        public string Name { get; set; }

        public decimal Mass { get; set; }

        public decimal EquatorialDiameter { get; set; }

        public int SurfaceTemperature { get; set; }

        [Required]
        public DateTime DiscoveryDate { get; set; }
    }
}