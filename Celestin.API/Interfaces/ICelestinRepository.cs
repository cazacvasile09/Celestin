using System.Collections;
using System.Collections.Generic;

namespace Celestin.API.Interfaces
{
    public interface ICelestinRepository
    {
        IEnumerable<DbModels.Celestin> GetCelestins();

        DbModels.Celestin GetCelestin(int id, bool includeDiscovery);

        IEnumerable<DbModels.Celestin> GetCelestinsByName(string name);

        void AddNewCelestin(DbModels.Celestin newCelestin);

        void UpdateCelestin(DbModels.Celestin updateCelestin);

        public IEnumerable<DbModels.Celestin> GetCelestinsByCountry(string countryName);

        void DeleteCelestin(DbModels.Celestin deleteCelestin);
        bool Save();
    }
}
