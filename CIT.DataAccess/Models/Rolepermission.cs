using System;
using System.Collections.Generic;

#nullable disable

namespace CIT.DataAccess.Models
{
    public partial class Rolepermission
    {
        public Rolepermission()
        {
            Id = new Random().Next(0, 5043232).ToString();
        }
        public string Id { get; set; }
        public string RoleId { get; set; }
        public string PermissionName { get; set; }
        public string Page { get; set; }

        public virtual Role Role { get; set; }
    }
}
