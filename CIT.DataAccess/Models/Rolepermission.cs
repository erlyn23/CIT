using System;
using System.Collections.Generic;

#nullable disable

namespace CIT.DataAccess.Models
{
    public partial class Rolepermission
    {
        public int Id { get; set; }
        public int RoleId { get; set; }
        public string PermissionName { get; set; }

        public virtual Role Role { get; set; }
    }
}
