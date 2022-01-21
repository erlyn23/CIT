using CIT.BusinessLogic.Contracts;
using CIT.Dtos.Requests;
using CIT.Presentation.Filters;
using CIT.Tools;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CIT.Presentation.Controllers
{
    [ServiceFilter(typeof(ExceptionFilter))]
    public class VehicleController : BaseCITController
    {
        private readonly IVehicleService _vehicleService;
        private readonly TokenCreator _tokenCreator;
        public VehicleController(TokenCreator tokenCreator, IRolePermissionService rolePermissionService, IVehicleService vehicleService) : base(rolePermissionService, tokenCreator)
        {
            _vehicleService = vehicleService;
            _tokenCreator = tokenCreator;
        }
        public IActionResult Index()
        {
            return View();
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet]
        [ServiceFilter(typeof(AuthFilter))]
        public async Task<IActionResult> GetVehiclesAsync()
        {
            var lenderBusinessId = await _tokenCreator.GetLenderBusinessId(Request);
            var vehicles = await _vehicleService.GetVehiclesAsync(lenderBusinessId);
            return Json(vehicles);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        [ServiceFilter(typeof(AuthFilter))]
        public async Task<IActionResult> SaveVehicleAsync(VehicleDto vehicle)
        {
            var lenderBusinessId = await _tokenCreator.GetLenderBusinessId(Request);
            return Json(await _vehicleService.AddVehicleAsync(vehicle, lenderBusinessId));
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpPost]
        [ServiceFilter(typeof(AuthFilter))]
        public async Task<IActionResult> UpdateVehicleAsync(VehicleDto vehicle)
        {
            return Json(await _vehicleService.UpdateVehicleAsync(vehicle));
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet]
        [ServiceFilter(typeof(AuthFilter))]
        public async Task<IActionResult> DeleteVehicleAsync(int id)
        {
            await _vehicleService.DeleteVehicleAsync(id);
            return Json("Vehículo eliminado correctamente");
        }
    }
}
