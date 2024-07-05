﻿using System.Collections;
using System.Collections.Generic;

namespace Celestin.API.Interfaces
{
    public interface ICelestinRepository
    {
        IEnumerable<DbModels.Celestin> GetCelestins();

        DbModels.Celestin GetCelestin(int id, bool includeDiscovery);

        IEnumerable<DbModels.Celestin> GetCelestinsByName(string name);
        IEnumerable<DbModels.Celestin> GetCelestinsByCountry(string country);

        void AddNewCelestin(DbModels.Celestin celestin);
        void UpdateCelestin(DbModels.Celestin celestin);
        string GetCountryWithMostBlackHoleDiscoveries();



        bool Save();
    }
}