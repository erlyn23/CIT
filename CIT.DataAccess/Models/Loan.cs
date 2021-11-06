using System;
using System.Collections.Generic;

#nullable disable

namespace CIT.DataAccess.Models
{
    public partial class Loan
    {
        public Loan()
        {
            Payments = new HashSet<Payment>();

        }

        public int Id { get; set; }
        public int DuesQuantity { get; set; }
        public decimal TotalLoan { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int PayDay { get; set; }
        public decimal InterestRate { get; set; }
        public decimal MensualPay { get; set; }
        public int LenderBusinessId { get; set; }
        public int EntityInfoId { get; set; }

        public virtual Entitiesinfo EntityInfo { get; set; }
        public virtual ICollection<Payment> Payments { get; set; }
        public virtual LenderBusiness LenderBusiness { get; set; }
    }
}
