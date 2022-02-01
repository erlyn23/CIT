using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIT.Dtos.Requests
{
    public class LoanDto
    {
        public int Id { get; set; }
        public int DuesQauntity { get; set; }
        public decimal TotalLoan { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int PayDay { get; set; }
        public decimal InterestRate { get; set; }
        public decimal MensualPay { get; set; }

        public int LenderBusinessId { get; set; }
        public int EntityInfoId { get; set; }

        public LenderBusinessDto LenderBusiness { get; set; }
        public EntityInfoDto entityInfo { get; set; }

    }
}
