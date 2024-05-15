using System;
using System.Collections.Generic;

namespace Celestin.API.DbModels
{
    public partial class Type
    {
        public Type()
        {
            DiscoverySource = new HashSet<DiscoverySource>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public virtual ICollection<DiscoverySource> DiscoverySource { get; set; }
    }
}
