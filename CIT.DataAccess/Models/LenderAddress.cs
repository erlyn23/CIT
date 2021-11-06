using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIT.DataAccess.Models
{
    public class LenderAddress
    {
        public int Id { get; set; }
        public int LenderBusinessId { get; set; }
        public int AddressId { get; set; }

        public Address Address { get; set; }
        public LenderBusiness LenderBusiness { get; set; }
    }
}
