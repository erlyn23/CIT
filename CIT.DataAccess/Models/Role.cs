using System;
using System.Collections.Generic;

#nullable disable

namespace CIT.DataAccess.Models
{
    public partial class Role
    {
        public Role()
        {
            Rolepermissions = new HashSet<Rolepermission>();
            Userroles = new HashSet<Userrole>();
        }

        public int Id { get; set; }
        public string RoleName { get; set; }
        public int EntityInfoId { get; set; }

        public virtual Entitiesinfo EntityInfo { get; set; }
        public virtual ICollection<Rolepermission> Rolepermissions { get; set; }
        public virtual ICollection<Userrole> Userroles { get; set; }
    }
}
