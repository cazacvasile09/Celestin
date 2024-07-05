﻿using Celestin.API.Common;
using Celestin.API.DbModels;
using Celestin.API.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace Celestin.API.CelestinClassifier
{
    public class Planets : IAbstractType, IVerification
    {
        private readonly NasaContext ctx;

        public Planets(NasaContext _ctx)
        {
            ctx = _ctx;
        }

        public List<DbModels.Celestin> GetCelestins()
        {
            var items = ctx.Celestin.Include(c => c.DiscoverySource).OrderBy(x => x.Name);

            return items.Where(IsPartOfObject).ToList();
        }

        public bool IsPartOfObject(DbModels.Celestin item)
        {
            var massLimit = Commons.massOfJupiter * 13;

            if (item.Mass < massLimit)
                return true;
            return false;
        }
    }
}