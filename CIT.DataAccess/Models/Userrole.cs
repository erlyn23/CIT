using System;
using System.Collections.Generic;

#nullable disable

namespace CIT.DataAccess.Models
{
    public partial class Userrole
    {
        public Userrole()
        {
            Id = new Random().Next(0, 5043232).ToString();
        }
        public string Id { get; set; }
        public string RoleId { get; set; }
        public string UserId { get; set; }
        public string EntityInfoId { get; set; }

        public virtual Entitiesinfo EntityInfo { get; set; }
        public virtual Role Role { get; set; }
        public virtual User User { get; set; }
    }
}
