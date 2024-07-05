using Celestin.API.Common;
using Celestin.API.DbModels;
using Celestin.API.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Celestin.API.Repositories
{
    public class CelestinRepository : ICelestinRepository
    {


        public string GetCountryWithMostBlackHoleDiscoveries()
        {
            var blackHoles = new ConcreteFactory(ctx).GetCelestins(Commons.BlackHole);
            var countryBlackHoleCount = blackHoles
                .GroupBy(c => c.DiscoverySource.StateOwner)
                .Select(g => new { Country = g.Key, Count = g.Count() })
                .OrderByDescending(g => g.Count)
                .FirstOrDefault();

            return countryBlackHoleCount?.Country;
        }


        public void AddNewCelestin(DbModels.Celestin celestin)
        {
            if (celestin == null)
            {
                throw new ArgumentNullException(nameof(celestin));
            }

            ctx.Celestin.Add(celestin);
        }

        public void UpdateCelestin(DbModels.Celestin celestin)
        {
            if (celestin == null)
            {
                throw new ArgumentNullException(nameof(celestin));
            }

            ctx.Celestin.Update(celestin);
        }



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

       

        public IEnumerable<DbModels.Celestin> GetCelestinsByCountry(string country)
        {
            return ctx.Celestin.Include(x => x.DiscoverySource).Where(y => y.DiscoverySource.StateOwner.ToLower().Contains(country.ToLower())).ToList();
        }

        public bool Save()
        {
            return (ctx.SaveChanges() >= 0);
        }
         

    }
}
