using Celestin.API.DbModels;
using Celestin.API.Interfaces;
using System.Linq;

namespace Celestin.API.Repositories
{
    public class DiscoveryRepository : IDiscoveryRepository
    {
        private readonly NasaContext ctx;

        public DiscoveryRepository(NasaContext _ctx)
        {
            ctx = _ctx;
        }

        public bool ExistDiscovery(int id)
        {
            return ctx.DiscoverySource.Any(x => x.Id == id);
        }

    }
}
