using Celestin.API.Common;
using Celestin.API.DbModels;
using Celestin.API.Interfaces;
using Microsoft.EntityFrameworkCore;
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

        public void AddNewCelestin(DbModels.Celestin newCelestin)
        {
            ctx.Celestin.Add(newCelestin);
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

            if (!string.IsNullOrEmpty(name))
            { return ctx.Celestin.Include(x => x.DiscoverySource).Where(y => y.Name.ToLower().Contains(name.ToLower())).ToList(); }

            else
            {

                //return new ErrorResult { ErrorMessage = "Field 'Name' must be completed" };
                return ctx.Celestin;
            }
        }


            public IEnumerable<DbModels.Celestin> GetCelestinsByCountry(string country)
        {
            return ctx.Celestin.Include(c => c.DiscoverySource).Where(c => c.DiscoverySource.StateOwner.ToLower() == country.ToLower()).ToList();
        }

        public string GetCountryWithMostBlackHoleDiscoveries()
        {
            var blackHoles = new ConcreteFactory(ctx).GetCelestins(Commons.BlackHole);
            var countryBlackHoleCount = blackHoles.GroupBy(c => c.DiscoverySource.StateOwner).Select(g => new { Country = g.Key, Count = g.Count() }).OrderByDescending(g => g.Count).FirstOrDefault();

            return countryBlackHoleCount?.Country;
        }

        public void DeleteCelestin(DbModels.Celestin celestin)
        {
            ctx.Celestin.Remove(celestin);
            Save();
        }


        public bool Save()
        {
            return (ctx.SaveChanges() >= 0);
        }
        
        public void UpdateCelestin(DbModels.Celestin celestin)
        {
            ctx.Celestin.Update(celestin);
            Save();
        }
        
    }
}
