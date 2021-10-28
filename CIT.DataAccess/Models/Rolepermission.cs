using System;
using System.Collections.Generic;

#nullable disable

namespace CIT.DataAccess.Models
{
    public partial class Rolepermission
    {
        public Rolepermission()
        {

            Id = Guid.NewGuid().ToString().Trim('-');
        }
        public string Id { get; set; }
        public string RoleId { get; set; }
        public string OperationId { get; set; }
        public string PageId { get; set; }

        public virtual Operation Operation { get; set; }
        public virtual Page Page { get; set; }
        public virtual Role Role { get; set; }
    }
}
