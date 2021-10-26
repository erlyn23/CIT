using System;
using System.Collections.Generic;

#nullable disable

namespace CIT.DataAccess.Models
{
    public partial class Useraddress
    {
        public Useraddress()
        {
            Id = new Random().Next(0, 5043232).ToString();
        }
        public string Id { get; set; }
        public string UserId { get; set; }
        public string AddressId { get; set; }

        public virtual Address Address { get; set; }
        public virtual User User { get; set; }
    }
}
