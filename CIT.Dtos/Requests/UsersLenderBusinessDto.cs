using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIT.Dtos.Requests
{
    public class UsersLenderBusinessDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int LenderBusinessId { get; set; }
    }
}
