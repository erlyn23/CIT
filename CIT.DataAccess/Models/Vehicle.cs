using System;
using System.Collections.Generic;

#nullable disable

namespace CIT.DataAccess.Models
{
    public partial class Vehicle
    {
        public Vehicle()
        {
            Id = new Random().Next(0, 5043232).ToString();
        }
        public string Id { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public string Enrollment { get; set; }
        public string LicensePlate { get; set; }
        public string Color { get; set; }
        public int Year { get; set; }
        public string EntityInfoId { get; set; }

        public virtual Entitiesinfo EntityInfo { get; set; }
    }
}
