using CIT.DataAccess.DbContexts;
using CIT.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIT.Dtos.Requests
{
    public class LenderBusinessDto
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Debes escribir el nombre del negocio")]
        public string BusinessName { get; set; }
        [Required(ErrorMessage = "Debes escribir tu Rnc")]
        [MinLength(11, ErrorMessage = "El RNC solo debe contener 11 números")]
        [MaxLength(11, ErrorMessage = "El RNC solo debe contener 11 números")]
        [RegularExpression("[0-9]*", ErrorMessage = "Solo puedes escribir números")]
        [ValidateExists("Rnc", "El usuario con este Rnc ya existe, intenta con uno nuevo")]
        public string Rnc { get; set; }
        [Required(ErrorMessage = "Debes escribir tu teléfono")]
        [MinLength(10, ErrorMessage = "El teléfono solo debe contener 10 números")]
        [MaxLength(10, ErrorMessage = "El teléfono solo debe contener 10 números")]
        [RegularExpression("[0-9]*", ErrorMessage = "Solo puedes escribir números")]
        [ValidateExists("Phone", "El usuario con este teléfono ya existe, intenta con uno nuevo")]
        public string Phone { get; set; }
        [Required(ErrorMessage = "Debes escribir tu correo")]
        [EmailAddress(ErrorMessage = "Debes escribir un correo válido")]
        [ValidateExists("Email", "El usuario con este email ya existe, intenta con uno nuevo")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Debes escribir tu contraseña")]
        [RegularExpression("^(?=.*[0-9])(?=.*[a-z])(?=.*[A-Z]).{6,20}$", ErrorMessage = "Debes escribir una contraseña segura, aségurate de combinar números, mayúsculas y carácteres")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Debes confirmar tu contraseña")]
        [RegularExpression("^(?=.*[0-9])(?=.*[a-z])(?=.*[A-Z]).{6,20}$", ErrorMessage = "La confirmación de contraseña no es segura")]
        public string ConfirmPassword { get; set; }
        public string Photo { get; set; }
        public AddressDto Address { get; set; }


        private abstract class UniqueValidation : ValidationAttribute
        {
            protected string Field { get; set; }
            protected string ErrorMsg { get; set; }
            protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            {
                var dbContext = (CentroInversionesTecnocorpDbContext)validationContext.GetService(typeof(CentroInversionesTecnocorpDbContext));

                var sendedValue = value.ToString();
                var lenderBusiness = new LenderBusiness();

                switch (Field)
                {
                    case "Email":
                        lenderBusiness = dbContext.LenderBusinesses.Where(u => u.Email.Equals(sendedValue)).FirstOrDefault();
                        break;
                    case "Phone":
                        lenderBusiness = dbContext.LenderBusinesses.Where(u => u.Phone.Equals(sendedValue)).FirstOrDefault();
                        break;
                    case "Rnc":
                        lenderBusiness = dbContext.LenderBusinesses.Where(u => u.Rnc.Equals(sendedValue)).FirstOrDefault();
                        break;
                }


                if (lenderBusiness != null)
                    return new ValidationResult(ErrorMsg);

                return ValidationResult.Success;
            }
        }

        private class ValidateExists : UniqueValidation
        {
            public ValidateExists(string field, string errorMsg)
            {
                Field = field;
                ErrorMsg = errorMsg;
            }
        }
    }
}
