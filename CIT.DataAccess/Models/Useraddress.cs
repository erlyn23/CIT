using System;
using System.Collections.Generic;

#nullable disable

namespace CIT.DataAccess.Models
{
    public partial class Useraddress
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int AddressId { get; set; }

        public virtual Address Address { get; set; }
        public virtual User User { get; set; }
    }
}
