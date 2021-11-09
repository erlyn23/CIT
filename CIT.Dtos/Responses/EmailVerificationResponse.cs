using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIT.Dtos.Responses
{
    public class EmailVerificationResponse
    {
        public string UserEmail { get; set; }
        public string RandomCode { get; set; }
        public DateTime ExpireDate { get; set; }
    }
}
