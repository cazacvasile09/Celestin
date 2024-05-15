using System;
using System.Collections.Generic;

namespace Celestin.API.DbModels
{
    public partial class DiscoverySource
    {
        public DiscoverySource()
        {
            Celestin = new HashSet<Celestin>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime EstablishmentDate { get; set; }
        public string StateOwner { get; set; }
        public int TypeId { get; set; }

        public virtual Type Type { get; set; }
        public virtual ICollection<Celestin> Celestin { get; set; }
    }
}
