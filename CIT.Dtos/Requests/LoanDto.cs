using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIT.Dtos.Requests
{
    public class LoanDto
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "El nombre del préstamo es requerido")]
        [MaxLength(20, ErrorMessage = "El nombre del préstamo debe ser un identificador de no más de 20 carácteres")]
        public string LoanName { get; set; }
        [Required(ErrorMessage = "Es necesario una descripción del préstamo")]
        [MaxLength(250, ErrorMessage = "Solo puedes escribir 250 carácteres")]
        public string Description { get; set; }
        [Required(ErrorMessage = "La cantidad de cuotas es obligatoria")]
        [NotNegativeOrZeroValidation(ErrorMessage = "El número no puede ser 0 o negativo")]
        public int DuesQuantity { get; set; }
        [Required(ErrorMessage = "El préstamo total es obligatorio")]
        [NotNegativeOrZeroValidation(ErrorMessage = "El préstamo total no puede ser 0 o negativo")]
        public decimal TotalLoan { get; set; }
        [Required(ErrorMessage = "Debes ingresar una fecha de inicio")]
        public DateTime StartDate { get; set; }
        [Required(ErrorMessage = "Debes ingresar una fecha final")]
        public DateTime EndDate { get; set; }
        [Required(ErrorMessage = "Debes escribir el día de pago")]
        [NotNegativeOrZeroValidation(ErrorMessage = "El día de pago no puede ser 0 o negativo")]
        [Range(1, 31, ErrorMessage = "El día de pago debe estar entre 1 y 31")]
        public int PayDay { get; set; }
        [Required(ErrorMessage = "Debes escribir la tasa de interés")]
        [NotNegativeOrZeroValidation(ErrorMessage = "La tasa de interés no puede ser 0 o negativo")]
        public decimal InterestRate { get; set; }
        [Required(ErrorMessage = "Debes escribir el pago mensual")]
        [NotNegativeOrZeroValidation(ErrorMessage = "El pago mensual no puede ser 0 o negativo")]
        public decimal MensualPay { get; set; }
        public int LenderBusinessId { get; set; }
        public int EntityInfoId { get; set; }
        [Required(ErrorMessage = "El usuario debe ser obligatorio")]
        [NotNegativeOrZeroValidation(ErrorMessage = "El usuario no puede ser 0 o negativo")]
        public int UserId { get; set; }

        public LenderBusinessDto LenderBusiness { get; set; }
        public EntityInfoDto entityInfo { get; set; }


        private class NotNegativeOrZeroValidation : ValidationAttribute
        {
            public override bool IsValid(object value)
            {
                int.TryParse(value.ToString(), out int duesQuantity);
                return duesQuantity > 0;
            }
        }
    }
}
