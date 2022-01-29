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
        [RegularExpression("^[0-9]+$", ErrorMessage = "Solo puedes escribir números")]
        public string Rnc { get; set; }
        [Required(ErrorMessage = "Debes escribir tu teléfono")]
        [MinLength(10, ErrorMessage = "El teléfono solo debe contener 10 números")]
        [MaxLength(10, ErrorMessage = "El teléfono solo debe contener 10 números")]
        [RegularExpression("^[0-9]+$", ErrorMessage = "Solo puedes escribir números")]
        public string Phone { get; set; }
        [Required(ErrorMessage = "Debes escribir tu correo")]
        [EmailAddress(ErrorMessage = "Debes escribir un correo válido")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Debes escribir tu contraseña")]
        [RegularExpression("^(?=.*[0-9])(?=.*[a-z])(?=.*[A-Z]).{6,20}$", ErrorMessage = "Debes escribir una contraseña segura, aségurate de combinar números, mayúsculas y carácteres")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Debes confirmar tu contraseña")]
        [RegularExpression("^(?=.*[0-9])(?=.*[a-z])(?=.*[A-Z]).{6,20}$", ErrorMessage = "La confirmación de contraseña no es segura")]
        public string ConfirmPassword { get; set; }
        public string Photo { get; set; }
        public AddressDto Address { get; set; }
    }
}
