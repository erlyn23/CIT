using System;
using System.Collections.Generic;

#nullable disable

namespace CIT.DataAccess.Models
{
    public partial class Rolepermission
    {
        public int Id { get; set; }
        public int RoleId { get; set; }
        public int OperationId { get; set; }
        public int PageId { get; set; }

        public virtual Operation Operation { get; set; }
        public virtual Page Page { get; set; }
        public virtual Role Role { get; set; }
    }
}
