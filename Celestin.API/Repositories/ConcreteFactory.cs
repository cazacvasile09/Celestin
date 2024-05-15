using Celestin.API.CelestinClassifier;
using Celestin.API.Common;
using Celestin.API.DbModels;
using Celestin.API.Interfaces;
using System.Collections.Generic;

namespace Celestin.API.Repositories
{
    public class ConcreteFactory : IAbstractFactory
    {
        private readonly NasaContext ctx;

        public ConcreteFactory(NasaContext _ctx)
        {
            ctx = _ctx;
        }

        public List<DbModels.Celestin> GetCelestins(string type)
        {
            switch (type.ToLower())
            {
                case Commons.Planet:
                    return new Planets(ctx).GetCelestins();
                case Commons.Star:
                    return new Stars(ctx).GetCelestins();
                case Commons.BlackHole:
                    return new BlackHoles(ctx).GetCelestins();
                default:
                    return null;
            }
        }
    }
}
