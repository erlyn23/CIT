using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIT.Dtos.Responses
{
    public class DashboardDto
    {
        public int TotalLoansQuantity { get; set; }
        public int TotalPaymentsQuantity { get; set; }
        public int TotalUsersQuantity { get; set; }
        public int TotalVehicleAssignmentsQuantity { get; set; }
        public int TotalDuesQuantity { get; set; }

        public List<PerDayDto> LoanQuantityPerDay { get; set; }
        public List<PerDayDto> PaymentQuantityPerDay { get; set; }
        public List<PerDayDto> UserQuantityPerDay { get; set; }
        public List<PerDayDto> VehicleAssignmentsQuantityPerDay { get; set; }
        public List<PerDayDto> DuesQuantityPerDay { get; set; }

        public DashboardDto()
        {
            LoanQuantityPerDay = new List<PerDayDto>();
            PaymentQuantityPerDay = new List<PerDayDto>();
            UserQuantityPerDay = new List<PerDayDto>();
            VehicleAssignmentsQuantityPerDay = new List<PerDayDto>(); 
            DuesQuantityPerDay = new List<PerDayDto>();
        }
    }
}
