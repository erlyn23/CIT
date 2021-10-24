using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIT.Dtos.Requests
{
    public class RoleDto
    {
        public int RoleId { get; set; }
        [Required(ErrorMessage = "Debes escribir un rol")]
        public string Role { get; set; }
        public List<RolePermissionDto> RolePermissions { get; set; }

        public RoleDto()
        {
            RolePermissions = new List<RolePermissionDto>();
        }
    }
}
