using Celestin.API.Models.CelestinModels;
using System.Collections;
using System.Collections.Generic;

namespace Celestin.API.Interfaces
{
    public interface ICelestinRepository
    {
        IEnumerable<DbModels.Celestin> GetCelestins();

        DbModels.Celestin GetCelestin(int id, bool includeDiscovery);

        IEnumerable<DbModels.Celestin> GetCelestinsByName(string name);

        void AddNewCelestin(DbModels.Celestin celestin);

        void UpdateCelestin(DbModels.Celestin celestin);

        public IEnumerable<DbModels.Celestin> GetCelestinsByCountry(string name);

        string GetCountryWithMostBlackHoleDiscoveries();
        bool Save();
        void DeleteById(DbModels.Celestin celestin);
    }
}
