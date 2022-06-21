using System;
using System.Collections.Generic;

#nullable disable

namespace CIT.DataAccess.Models
{
    public partial class User
    {
        public User()
        {
            UsersLenderBusinesses = new HashSet<UsersLenderBusinesses>();
            Payments = new HashSet<Payment>();
            Loans = new HashSet<Loan>();
            VehicleAssignments = new HashSet<VehicleAssignment>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string IdentificationDocument { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Photo { get; set; }
        public int EntityInfoId { get; set; }

        public virtual Entitiesinfo EntityInfo { get; set; }
        public virtual Userrole Userrole { get; set; }
        public virtual ICollection<Payment> Payments { get; set; }
        public virtual ICollection<Loan> Loans { get; set; }
        public virtual Useraddress Useraddress { get; set; }
        public virtual ICollection<VehicleAssignment> VehicleAssignments { get; set; }
        public virtual ICollection<UsersLenderBusinesses> UsersLenderBusinesses { get; set; }
    }
}
