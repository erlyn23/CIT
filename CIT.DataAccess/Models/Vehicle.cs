using System;
using System.Collections.Generic;

#nullable disable

namespace CIT.DataAccess.Models
{
    public partial class Vehicle
    {
        public int Id { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public string Enrollment { get; set; }
        public string LicensePlate { get; set; }
        public string Color { get; set; }
        public int Year { get; set; }
        public int EntityInfoId { get; set; }

        public virtual Entitiesinfo EntityInfo { get; set; }
    }
}
