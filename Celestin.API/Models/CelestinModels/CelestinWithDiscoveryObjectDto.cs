﻿using System;

namespace Celestin.API.Models.CelestinModels
{
    public class CelestinWithDiscoveryObjectDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public decimal Mass { get; set; }

        public decimal EquatorialDiameter { get; set; }

        public int SurfaceTemperature { get; set; }

        public DateTime DiscoveryDate { get; set; }

        public DiscoverySourceDto Discovery { get; set; }
    }
}
