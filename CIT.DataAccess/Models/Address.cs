using System;
using System.Collections.Generic;

#nullable disable

namespace CIT.DataAccess.Models
{
    public partial class Address
    {
        public Address()
        {
            Useraddresses = new HashSet<Useraddress>();
        }

        public int Id { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public string Province { get; set; }
        public string Street1 { get; set; }
        public string Street2 { get; set; }
        public int HouseNumber { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public int EntityInfoId { get; set; }

        public virtual Entitiesinfo EntityInfo { get; set; }
        public virtual ICollection<Useraddress> Useraddresses { get; set; }
    }
}
