using System.Collections.Generic;

namespace Celestin.API.Interfaces
{
    public interface IAbstractFactory
    {
        List<DbModels.Celestin> GetCelestins(string type);
    }
}
