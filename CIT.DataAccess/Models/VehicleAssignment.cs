using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIT.DataAccess.Models
{
    public class VehicleAssignment
    {
        public int Id { get; set; } 
        public int UserId { get; set; }
        public int VehicleId { get; set; }
        
        public User User { get; set; }
        public Vehicle Vehicle { get; set; }
    }
}
