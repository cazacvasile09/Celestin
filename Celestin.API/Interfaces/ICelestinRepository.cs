using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Celestin.API.Interfaces
{
    public interface ICelestinRepository
    {
        IEnumerable<DbModels.Celestin> GetCelestins();

        DbModels.Celestin GetCelestin(int id, bool includeDiscovery);

        IEnumerable<DbModels.Celestin> GetCelestinsByName(string name);

        void AddNewCelestin(DbModels.Celestin newCelestin);

        void UpdateCelestin(DbModels.Celestin updateCelestin);

        void DeleteCelestin(DbModels.Celestin deleteCelestin);

        IEnumerable<DbModels.Celestin> GetCelestinsByCountry(string countryName);

        bool Save();
    }
}
