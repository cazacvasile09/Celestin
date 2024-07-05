using System.Collections;
using System.Collections.Generic;

namespace Celestin.API.Interfaces
{
    public interface ICelestinRepository
    {
        IEnumerable<DbModels.Celestin> GetCelestins();

        DbModels.Celestin GetCelestin(int id, bool includeDiscovery);

        IEnumerable<DbModels.Celestin> GetCelestinsByName(string name);
        IEnumerable<DbModels.Celestin> GetCelestinsByCountry(string country);

        bool Save();
        void AddNewCelestin(DbModels.Celestin newCelestin);
        void Update(DbModels.Celestin newCelestin);
        string GetCountryWithMostBlackHole();
        string DeleteCelestin(DbModels.Celestin Celestin);

       



    }
}
