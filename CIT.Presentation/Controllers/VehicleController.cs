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

        public VehicleController(IVehicleService vehicleService, TokenCreator tokenCreator, IRolePermissionService rolePermissionService) : base(rolePermissionService, tokenCreator)
        {
            _vehicleService = vehicleService;
            _tokenCreator = tokenCreator;
        }
        public IActionResult Index()
        {
            return View();
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ServiceFilter(typeof(AuthFilter))]
        [HttpGet]
        public async Task<IActionResult> GetVehiclesAsync()
        {
            int lenderBusinessId = await _tokenCreator.GetLenderBusinessId(Request);
            return Json(await _vehicleService.GetVehiclesAsync(lenderBusinessId));
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ServiceFilter(typeof(AuthFilter))]
        public async Task<IActionResult> DeleteVehicleAsync(int id)
        {
            await _vehicleService.DeleteVehicleAsync(id);
            return Json("Vehiculo eliminado correctamente");
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ServiceFilter(typeof(AuthFilter))]
        [HttpPost]
        public async Task<IActionResult> UpdateVehicleAsync([FromBody] VehicleDto vehicle)
        {
            if (ModelState.IsValid)
                return Json(await _vehicleService.UpdateVehicleAsync(vehicle));

            return BadRequest(ModelState.Values.ToList());
        }
    }
}
