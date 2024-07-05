using System;
using System.ComponentModel.DataAnnotations;

namespace Celestin.API.Models.CelestinModels
{
    public class CelestinForUpdateDto
    {
        public string Name { get; set; }

        public decimal Mass { get; set; }

        public decimal EquatorialDiameter { get; set; }

        public int SurfaceTemperature { get; set; }

        [Required]
        public DateTime DiscoveryDate { get; set; }

    }

}
