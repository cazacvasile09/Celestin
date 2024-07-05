using NLog.LayoutRenderers;
using System.Collections;
using System.Collections.Generic;

namespace Celestin.API.Interfaces
{
    public interface ICelestinRepository
    {
        IEnumerable<DbModels.Celestin> GetCelestins();

        DbModels.Celestin GetCelestin(int id, bool includeDiscovery);

        IEnumerable<DbModels.Celestin> GetCelestinsByName(string name);

        //IEnumerable<DbModels.Celestin> AddNewCelestin(DbModels.Celestin obiect);  //asta este prima incercare(fail)
        void AddNewCelestin(DbModels.Celestin c); //aici am scris antetul metodei(efectiv e de tip void si mergem cu un obiect de tip Celestin)
        //e bine macar ca am stiut unde trebuie sa o declar si implementez :))
        bool Save();

        void updateCelestin(DbModels.Celestin c);//functia pentru update la un object deja existent, se va lua dupa id

        void deleteCelestin(DbModels.Celestin c);
    }
}
