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
        public int Id { get; set; }
        public int RoleId { get; set; }
        [Required(ErrorMessage = "Debes seleccionar una operación")]
        public int OperationId { get; set; }
        public string OperationName { get; set; }
        [Required(ErrorMessage = "Debes seleccionar una página")]
        public int PageId { get; set; }
        public string PageName { get; set; }
        public string IconClass { get; set; }
        public string Route { get; set; }
    }
}
