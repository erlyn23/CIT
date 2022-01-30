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
    [PageFilter("Vehiculos")]
    public class VehicleAssignmentController : BaseCITController
    {
        private readonly TokenCreator _tokenCreator;
        private readonly IVehicleAssignmentService _vehicleAssignmentService;

        public VehicleAssignmentController(TokenCreator tokenCreator, IRolePermissionService rolePermissionService, IVehicleAssignmentService vehicleAssignmentService) : base(rolePermissionService, tokenCreator)
        {
            _tokenCreator = tokenCreator;
            _vehicleAssignmentService = vehicleAssignmentService;
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [OperationFilter("Obtener")]
        [ServiceFilter(typeof(AuthFilter))]
        [HttpGet]
        public async Task<IActionResult> GetVehicleAssignmentsAsync()
        {
            var lenderBusinessId = await _tokenCreator.GetLenderBusinessId(Request);
            return Json(await _vehicleAssignmentService.GetVehiclesAssignmentsAsync(lenderBusinessId));
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [OperationFilter("Agregar")]
        [ServiceFilter(typeof(AuthFilter))]
        [HttpPost]
        public async Task<IActionResult> AssignVehicleAsync([FromBody] VehicleAssignmentDto vehicleAssignment)
        {
            if (ModelState.IsValid)
                return Json(await _vehicleAssignmentService.AssignVehicleAsync(vehicleAssignment));

            return Json(ModelState.Values.Select(v => v.Errors.Select(e => e.ErrorMessage)).ToList());
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [OperationFilter("Modificar")]
        [ServiceFilter(typeof(AuthFilter))]
        [HttpPost]
        public async Task<IActionResult> UpdateAssignmentAsync([FromBody] VehicleAssignmentDto vehicleAssignment)
        {
            if (ModelState.IsValid)
                return Json(await _vehicleAssignmentService.UpdateAssignmentAsync(vehicleAssignment));

            return Json(ModelState.Values.Select(v => v.Errors.Select(e => e.ErrorMessage)).ToList());
        }


        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [OperationFilter("Eliminar")]
        [ServiceFilter(typeof(AuthFilter))]
        [HttpGet]
        public async Task<IActionResult> DeleteAssignmentAsync(int id)
        {
            await _vehicleAssignmentService.DeleteAssignmentAsync(id);
            return Json("Asignación eliminada correctamente");
        }

    }
}
