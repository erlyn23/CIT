using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIT.Dtos.Requests
{
    public class UserRoleDto
    {

        public int UserRoleId { get; set; }
        public int UserId { get; set; }
        [Required(ErrorMessage = "Debes especificar el rol del usuario")]
        public int RoleId { get; set; }
        public RoleDto Role { get; set; }
    }
}
