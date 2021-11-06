using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIT.Dtos.Requests
{
    public class AddressDto
    {

        public int Id { get; set; }
        [Required(ErrorMessage = "Debes elegir un país")]
        public string Country { get; set; }
        [Required(ErrorMessage = "Debes escribir la ciudad")]
        [MaxLength(ErrorMessage = "La ciudad no puede contener más de 50 letras")]
        public string City { get; set; }
        [Required(ErrorMessage = "Debes escribir la provincia o estado")]
        public string Province { get; set; }
        [Required(ErrorMessage = "Debes escribir al menos una calle")]
        public string Street1 { get; set; }
        public string Street2 { get; set; }
        [Required(ErrorMessage = "Debes escribir una calle")]
        [RegularExpression("[0-9]*", ErrorMessage = "Solo puedes escribir números")]
        public int HouseNumber { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public EntityInfoDto EntityInfo { get; set; }
    }
}
