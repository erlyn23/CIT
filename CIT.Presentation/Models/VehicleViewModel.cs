using CIT.BusinessLogic.Contracts;
using CIT.Dtos.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CIT.Presentation.Models
{
    public class VehicleViewModel
    {
        public List<UserDto> Users { get; set; }
        public List<VehicleDto> Vehicles { get; set; }

        public VehicleViewModel(List<UserDto> users, List<VehicleDto> vehicles)
        {
            Users = users;
            Vehicles = vehicles;
        }
    }
}
