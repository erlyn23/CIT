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
    public class RolesController : BaseCITController
    {
        private readonly IRoleService _roleService;
        private readonly TokenCreator _tokenCreator;

        public RolesController(IRoleService roleService, TokenCreator tokenCreator, IRolePermissionService rolePermissionService) : base(rolePermissionService, tokenCreator)
        {
            _roleService = roleService;
            _tokenCreator = tokenCreator;
        }
        public IActionResult Index()
        {
            return View();
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ServiceFilter(typeof(AuthFilter))]
        [HttpGet]
        public async Task<IActionResult> GetRolesAsync()
        {
            int lenderBusinessId = await _tokenCreator.GetLenderBusinessId(Request);
            return Json(await _roleService.GetRolesAsync(lenderBusinessId));
        }


        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ServiceFilter(typeof(AuthFilter))]
        [HttpPost]
        public async Task<IActionResult> CreateRoleAsync([FromBody] RoleDto roleDto)
        {
            int lenderBusinessId = await _tokenCreator.GetLenderBusinessId(Request);
            if (ModelState.IsValid)
                return Json(await _roleService.CreateRoleAsync(roleDto, lenderBusinessId));

            return BadRequest(ModelState.Values.ToList());
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ServiceFilter(typeof(AuthFilter))]
        public async Task<IActionResult> DeleteRoleAsync(int id)
        {
            await _roleService.DeleteRoleAsync(id);
            return Json("Rol eliminado correctamente");
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ServiceFilter(typeof(AuthFilter))]
        [HttpPost]
        public async Task<IActionResult> UpdateRoleAsync([FromBody] RoleDto role)
        {
            if (ModelState.IsValid)
                return Json(await _roleService.UpdateRoleAsync(role));

            return BadRequest(ModelState.Values.ToList());
        }
    }
}
