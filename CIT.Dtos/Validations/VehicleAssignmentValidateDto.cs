using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIT.Dtos.Validations
{
    public class VehicleAssignmentValidateDto
    {
        public int VehicleId { get; set; }
        public int UserId { get; set; }
        public int LenderBusinessId { get; set; }
        public int VehicleAssignmentId { get; set; }
    }
}
