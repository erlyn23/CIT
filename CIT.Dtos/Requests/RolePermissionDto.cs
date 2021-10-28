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
        public string OperationId { get; set; }
        public string OperationName { get; set; }
        public string PageId { get; set; }
        public string PageName { get; set; }
    }
}
