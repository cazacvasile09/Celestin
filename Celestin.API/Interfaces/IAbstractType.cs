using System.Collections.Generic;

namespace Celestin.API.Interfaces
{
    public interface IAbstractType
    {
        List<DbModels.Celestin> GetCelestins();
    }
}
