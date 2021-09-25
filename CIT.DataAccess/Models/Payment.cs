using System;
using System.Collections.Generic;

#nullable disable

namespace CIT.DataAccess.Models
{
    public partial class Payment
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int LoanId { get; set; }
        public DateTime Date { get; set; }
        public decimal Pay { get; set; }
        public int EntityInfoId { get; set; }

        public virtual Entitiesinfo EntityInfo { get; set; }
        public virtual Loan Loan { get; set; }
        public virtual User User { get; set; }
    }
}
