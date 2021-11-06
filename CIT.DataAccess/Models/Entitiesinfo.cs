using System;
using System.Collections.Generic;

#nullable disable

namespace CIT.DataAccess.Models
{
    public partial class Entitiesinfo
    {
        public Entitiesinfo()
        {
            Addresses = new HashSet<Address>();
            Loans = new HashSet<Loan>();
            Payments = new HashSet<Payment>();
            Roles = new HashSet<Role>();
            Userroles = new HashSet<Userrole>();
            Users = new HashSet<User>();
            Vehicles = new HashSet<Vehicle>();
            LenderBusinesses = new HashSet<LenderBusiness>();

        }

        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public short Status { get; set; }

        public virtual ICollection<Address> Addresses { get; set; }
        public virtual ICollection<Loan> Loans { get; set; }
        public virtual ICollection<Payment> Payments { get; set; }
        public virtual ICollection<Role> Roles { get; set; }
        public virtual ICollection<Userrole> Userroles { get; set; }
        public virtual ICollection<User> Users { get; set; }
        public virtual ICollection<Vehicle> Vehicles { get; set; }
        public virtual ICollection<LenderBusiness> LenderBusinesses { get; set; }
    }
}
