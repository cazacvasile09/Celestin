using Celestin.API.Common;
using Celestin.API.DbModels;
using Celestin.API.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Celestin.API.CelestinClassifier
{
    public class BlackHoles : IAbstractType, IVerification
    {
        private readonly NasaContext ctx;

        public BlackHoles(NasaContext _ctx)
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
            var radius = item.EquatorialDiameter / 2;
            double schwarzschildRadius = (2 * Commons.gravitationalConstant * item.Mass) / Math.Pow(Commons.speedOfLight, 2);

            if (radius < schwarzschildRadius)
                return true;
            return false;
        }
    }
}
