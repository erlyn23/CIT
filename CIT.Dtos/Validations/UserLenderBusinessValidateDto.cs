using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIT.Dtos.Validations
{
    public class UserLenderBusinessValidateDto
    {
        public string Email { get; set; }
        public string IdentificationDocument { get; set; }
        public string Phone { get; set; }
        public int UserId { get; set; }
        public int LenderBusinessId { get; set; }

        public UserLenderBusinessValidateDto()
        {
            UserId = 0;
        }
    }
}
