using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIT.Dtos.Requests
{
    public class RolePermissionDto
    {
        public string Id { get; set; }
        public string RoleId { get; set; }
        public string PermissionName { get; set; }
        public string Page { get; set; }
    }
}
