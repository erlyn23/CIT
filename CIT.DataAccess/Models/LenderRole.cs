using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIT.DataAccess.Models
{
    public class LenderRole
    {
        public int Id { get; set; }
        public int LenderBusinessId { get; set; }
        public int RoleId { get; set; }

        public virtual Role Role { get; set; }
        public virtual LenderBusiness LenderBusiness { get; set; }
    }
}
