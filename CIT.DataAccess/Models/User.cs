using System;
using System.Collections.Generic;

#nullable disable

namespace CIT.DataAccess.Models
{
    public partial class User
    {
        public User()
        {
            Logs = new HashSet<Log>();
            Payments = new HashSet<Payment>();

        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string IdentificationDocument { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Photo { get; set; }
        public int LenderBusinessId { get; set; }
        public int EntityInfoId { get; set; }

        public virtual Entitiesinfo EntityInfo { get; set; }
        public virtual Userrole Userrole { get; set; }
        public virtual ICollection<Log> Logs { get; set; }
        public virtual ICollection<Payment> Payments { get; set; }
        public virtual Useraddress UserAddress { get; set; }
        public virtual VehicleAssignment VehicleAssignment { get; set; }
        public virtual LenderBusiness LenderBusiness { get; set; }
    }
}
