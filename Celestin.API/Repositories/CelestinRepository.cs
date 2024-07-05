﻿using Celestin.API.DbModels;
using Celestin.API.Interfaces;
using Microsoft.EntityFrameworkCore;
using NLog.LayoutRenderers;
using System.Collections.Generic;
using System.Linq;

namespace Celestin.API.Repositories
{
    public class CelestinRepository : ICelestinRepository
    {
        private readonly NasaContext ctx;

        public CelestinRepository(NasaContext _ctx)
        {
            ctx = _ctx;
        }

        public DbModels.Celestin GetCelestin(int id, bool includeDiscovery)
        {
            if (includeDiscovery)
            {
                return ctx.Celestin.Include(x => x.DiscoverySource).Include(z => z.DiscoverySource.Type).Where(w => w.Id == id).FirstOrDefault();
            }

            return ctx.Celestin.Where(w => w.Id == id).FirstOrDefault();
        }

        public IEnumerable<DbModels.Celestin> GetCelestins()
        {
            return ctx.Celestin.Include(c => c.DiscoverySource).OrderBy(x => x.Name).ToList();
        }

        public IEnumerable<DbModels.Celestin> GetCelestinsByName(string name)
        {
            return ctx.Celestin.Include(x => x.DiscoverySource).Where(y => y.Name.ToLower().Contains(name.ToLower())).ToList();
        }
        //public IEnumerable<DbModels.Celestin> AddNewCelestin(DbModels.Celestin obiect) //prima incercare(fail)
        //{
        //    return ctx.Celestin.Include(z => z.DiscoverySource)
        //}
        public void AddNewCelestin(DbModels.Celestin c) //metoda cu care adaugam in Celestin un obiect de tip Celestin
        {
            ctx.Celestin.Add(c); //sincer nu ma asteptam sa existe o functie Add si atat, credeam ca e mult mai complicat
        }
        public bool Save()
        {
            return (ctx.SaveChanges() >= 0);
        }
        public void updateCelestin(DbModels.Celestin c) //functia care face update la obiect cu noile informatii
        {
            ctx.Celestin.Update(c);
        }
    }
}
