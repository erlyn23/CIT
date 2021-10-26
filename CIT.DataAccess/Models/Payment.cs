using System;
using System.Collections.Generic;

#nullable disable

namespace CIT.DataAccess.Models
{
    public partial class Payment
    {
        public Payment()
        {
            Id = new Random().Next(0, 5043232).ToString();
        }
        public string Id { get; set; }
        public string UserId { get; set; }
        public string LoanId { get; set; }
        public DateTime Date { get; set; }
        public decimal Pay { get; set; }
        public string EntityInfoId { get; set; }

        public virtual Entitiesinfo EntityInfo { get; set; }
        public virtual Loan Loan { get; set; }
        public virtual User User { get; set; }
    }
}
