using System;
using System.Collections.Generic;

#nullable disable

namespace CIT.DataAccess.Models
{
    public partial class Page
    {
        public Page()
        {
            Rolepermissions = new HashSet<Rolepermission>();

            Id = Guid.NewGuid().ToString().Trim('-');
        }

        public string Id { get; set; }
        public string PageName { get; set; }

        public virtual ICollection<Rolepermission> Rolepermissions { get; set; }
    }
}
