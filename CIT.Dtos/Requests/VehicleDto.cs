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
        public int VehicleId { get; set; }
        public int LenderBusinessId { get; set; }
       
        [Required(ErrorMessage ="Debes de escribir la marca del vehículo")]
        public string Brand { get; set; }
        [Required(ErrorMessage ="Debes de escribir el modelo del vehículo")]
        public string Model { get; set; }
        [Required(ErrorMessage = "Debes de escribir la matrícula del vehículo")]
        [RegularExpression("[0-9]*", ErrorMessage = "Solo puedes escribir números")]
        [MinLength(10, ErrorMessage = "La matrícula solo debe contener 10 números")]
        [MaxLength(10, ErrorMessage = "La matrícula solo debe contener 10 números")]
        public int Enrollment { get; set; }
        [Required(ErrorMessage = "Debes de escribir la placa del vehículo")]
        [MinLength(7, ErrorMessage = "La placa solo debe contener 7 carácteres")]
        [MaxLength(7, ErrorMessage = "La placa solo debe contener 7 carácteres")]
        public int LicensePlate { get; set; }
        [Required(ErrorMessage = "Debes de escribir el color del vehículo")]
        [MinLength(15, ErrorMessage = "El color solo debe contener 15 carácteres")]
        [MaxLength(15, ErrorMessage = "El color solo debe contener 15 carácteres")]
        public string Color { get; set; }
        [Required(ErrorMessage = "Debes de escribir el año del vehículo")]
        [RegularExpression("[0-9]*", ErrorMessage = "Solo puedes escribir números")]
        [MinLength(11, ErrorMessage = "El año solo debe contener 11 números")]
        [MaxLength(11, ErrorMessage = "El año solo debe contener 11 números")]
        public int Year { get; set; }

        public EntityInfoDto EntityInfo { get; set; }

    }
}
