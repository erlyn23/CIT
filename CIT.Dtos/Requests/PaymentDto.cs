using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIT.Dtos.Requests
{
    public class PaymentDto
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "El usuario es obligatorio")]
        public int UserId { get; set; }
        [Required(ErrorMessage = "El préstamo es obligatorio")]
        public int LoanId { get; set; }
        [Required(ErrorMessage = "La fecha de pago es obligatoria")]
        public DateTime Date { get; set; }
        [Required(ErrorMessage = "El monto del pago es obligatorio")]
        [Range(0, double.PositiveInfinity, ErrorMessage = "Debes escribir un número mayor a 0")]
        public decimal Pay { get; set; }
        public int LenderBusinessId { get; set; }
        public int EntityInfoId { get; set; }

        public EntityInfoDto EntityInfo { get; set; }
        public LoanDto Loan { get; set; }
        public UserDto User { get; set; }
        public LenderBusinessDto LenderBusiness { get; set; }
    }
}
