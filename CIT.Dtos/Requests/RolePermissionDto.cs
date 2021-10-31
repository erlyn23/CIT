using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIT.Dtos.Requests
{
    public class RolePermissionDto
    {
        public string Id { get; set; }
        public string RoleId { get; set; }
        [Required(ErrorMessage = "Debes seleccionar una operación")]
        public string OperationId { get; set; }
        public string OperationName { get; set; }
        [Required(ErrorMessage = "Debes seleccionar una página")]
        public string PageId { get; set; }
        public string PageName { get; set; }
    }
}
