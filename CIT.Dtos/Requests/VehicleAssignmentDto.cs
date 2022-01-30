using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIT.Dtos.Requests
{
    public class VehicleAssignmentDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int VehicleId { get; set; }
        public DateTime AssignmentDate { get; set; }
        public string Comment { get; set; }
        
        public UserDto User { get; set; }
        public VehicleDto Vehicle { get; set; }
    }
}
