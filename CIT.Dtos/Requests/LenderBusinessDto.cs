using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIT.Dtos.Requests
{
    public class LenderBusinessDto
    {
        public int Id { get; set; }
        public string BusinessName { get; set; }
        public string Rnc { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
        public string Photo { get; set; }
        public int EntityInfoId { get; set; }
    }
}
