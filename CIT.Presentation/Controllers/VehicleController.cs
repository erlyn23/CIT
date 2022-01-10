using CIT.BusinessLogic.Contracts;
using CIT.Tools;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CIT.Presentation.Controllers
{
    public class VehicleController : BaseCITController
    {
        private readonly IVehicleService _vehicleService;
        public VehicleController(TokenCreator tokenCreator, IRolePermissionService rolePermissionService, IVehicleService vehicleService) : base(rolePermissionService, tokenCreator)
        {
            _vehicleService = vehicleService;
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
