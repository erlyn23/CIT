using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIT.Dtos.Responses
{
    public class AmortizationTableDto
    {
        public List<int> Periods { get; set; }
        public List<decimal> Dues { get; set; }
        public List<decimal> Interests { get; set; }
        public List<decimal> CapitalPayments { get; set; }
        public List<decimal> Balance { get; set; }

        public AmortizationTableDto()
        {
            Periods = new List<int>();
            Dues = new List<decimal>(); 
            Interests = new List<decimal>();    
            CapitalPayments = new List<decimal>();
            Balance = new List<decimal>();
        }
    }
}
