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
    public class VehicleDto
    {
        public int Id { get; set; }
        public int LenderBusinessId { get; set; }
       
        [Required(ErrorMessage ="Debes escribir la marca del vehículo")]
        public string Brand { get; set; }
        [Required(ErrorMessage ="Debes escribir el modelo del vehículo")]
        public string Model { get; set; }
        [Required(ErrorMessage = "Debes escribir la matrícula del vehículo")]
        [RegularExpression("^[0-9]+$", ErrorMessage = "La matrícula debe ser solo números")]
        public int Enrollment { get; set; }
        [Required(ErrorMessage = "Debes escribir la placa del vehículo")]
        [MaxLength(7, ErrorMessage = "La placa solo debe contener 7 carácteres")]
        public string LicensePlate { get; set; }
        [Required(ErrorMessage = "Debes escribir el color del vehículo")]
        [MaxLength(15, ErrorMessage = "El color solo debe contener 15 carácteres")]
        public string Color { get; set; }
        [Required(ErrorMessage = "Debes escribir el año del vehículo")]
        [YearValidation(ErrorMessage = "El año tiene que estar entre 1900 y {0}")]
        public int Year { get; set; }

        public int EntityInfoId { get; set; }
        public EntityInfoDto EntityInfo { get; set; }

        private class YearValidation : ValidationAttribute
        {
            protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            {
                var sendedValue = value.ToString();
                if (int.TryParse(sendedValue, out int vehicleYear))
                {
                    if (vehicleYear < 1900 || vehicleYear > DateTime.Now.Year)
                        return new ValidationResult(string.Format(ErrorMessage, DateTime.Now.Year.ToString()));
                }

                return ValidationResult.Success;
            }
        }

    }
}
