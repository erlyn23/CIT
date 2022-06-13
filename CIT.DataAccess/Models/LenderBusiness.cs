using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIT.DataAccess.Models
{
    public class LenderBusiness
    {
        public LenderBusiness()
        {
            UsersLenderBusinesses = new HashSet<UsersLenderBusinesses>();
            Roles = new HashSet<Role>();
            Loans = new HashSet<Loan>();
            Logs = new HashSet<Log>();
            Vehicles = new HashSet<Vehicle>();
        }
        public int Id { get; set; }
        public string BusinessName { get; set; }
        public string Rnc { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
        public string Photo { get; set; }
        public int EntityInfoId { get; set; }

        public virtual ICollection<UsersLenderBusinesses> UsersLenderBusinesses { get; set; }
        public virtual ICollection<Role> Roles { get; set; }
        public virtual ICollection<Loan> Loans { get; set; }
        public virtual ICollection<Log> Logs { get; set; }
        public virtual ICollection<Payment> Payments { get; set; }
        public virtual ICollection<Vehicle> Vehicles { get; set; }
        public virtual LenderAddress LenderAddress { get; set; }
        public virtual Entitiesinfo EntityInfo { get; set; }
        public virtual LenderRole LenderRole { get; set; }
    }
}
