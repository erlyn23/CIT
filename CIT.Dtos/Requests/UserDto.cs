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
    public class UserDto
    {
        public string Id { get; set; }
        [Required(ErrorMessage = "Debes escribir tu nombre")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Debes escribir tu apellido")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Debes escribir tu cédula")]
        [MinLength(11, ErrorMessage = "La cédula solo debe contener 11 números")]
        [MaxLength(11, ErrorMessage = "La cédula solo debe contener 11 números")]
        [RegularExpression("[0-9]*", ErrorMessage = "Solo puedes escribir números")]
        [ValidateExists("IdentificationDocument", "El usuario con esta cédula ya existe, intenta con uno nuevo")]
        public string IdentificationDocument { get; set; }
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


        private abstract class UniqueValidation : ValidationAttribute
        {
            protected string Field { get; set; }
            protected string ErrorMsg { get; set; }
            protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            {
                var dbContext = (CentroInversionesTecnocorpDbContext)validationContext.GetService(typeof(CentroInversionesTecnocorpDbContext));

                var sendedValue = value.ToString();
                var user = new User();
                switch (Field)
                {
                    case "Email":
                        user = dbContext.Users.Where(u => u.Email.Equals(sendedValue)).FirstOrDefault();
                        break;
                    case "Phone":
                        user = dbContext.Users.Where(u => u.Phone.Equals(sendedValue)).FirstOrDefault();
                        break;
                    case "IdentificationDocument":
                        user = dbContext.Users.Where(u => u.IdentificationDocument.Equals(sendedValue)).FirstOrDefault();
                        break;
                }

                if (user != null)
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
