using System;

namespace Celestin.API.Models
{
    public class DiscoverySourceDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime EstablishmentDate { get; set; }
        public string StateOwner { get; set; }
        public string Type { get; set; }
    }
}
