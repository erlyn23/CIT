using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIT.DataAccess.Models
{
    public class UsersLenderBusinesses
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int LenderBusinessId { get; set; }

        public virtual User User { get; set; }
        public virtual LenderBusiness LenderBusiness { get; set; }
    }
}
