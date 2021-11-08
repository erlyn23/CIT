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
        public List<RolePermissionDto> ToDelete { get; set; }
        public int LenderBusinessId { get; set; }
        public EntityInfoDto EntityInfo { get; set; }

        public RoleDto()
        {
            RolePermissions = new List<RolePermissionDto>();
            ToDelete = new List<RolePermissionDto>();
        }
    }
}
