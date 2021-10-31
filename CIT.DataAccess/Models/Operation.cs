using System;
using System.Collections.Generic;

#nullable disable

namespace CIT.DataAccess.Models
{
    public partial class Operation
    {
        public int Id { get; set; }
        public string OperationName { get; set; }

        public virtual Rolepermission Rolepermission { get; set; }
    }
}
