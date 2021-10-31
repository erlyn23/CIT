using System;
using System.Collections.Generic;

#nullable disable

namespace CIT.DataAccess.Models
{
    public partial class Operation
    {
        public Operation()
        {
            Rolepermissions = new HashSet<Rolepermission>();
        }
        public int Id { get; set; }
        public string OperationName { get; set; }

        public virtual ICollection<Rolepermission> Rolepermissions { get; set; }
    }
}
