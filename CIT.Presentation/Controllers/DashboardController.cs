using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CIT.Presentation.Filters;
using CIT.BusinessLogic.Contracts;
using CIT.BusinessLogic.Services;
using CIT.Dtos.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using CIT.Tools;

namespace CIT.Presentation.Controllers
{
    [ServiceFilter(typeof(ExceptionFilter))]
    [PageFilter("Dashboard")]
    public class DashboardController : BaseCITController
    {
        private readonly IRoleService _roleService;

        public DashboardController(IRoleService roleService, 
            TokenCreator tokenCreator, 
            IRolePermissionService rolePermissionService) : base(rolePermissionService, tokenCreator)
        {
            _roleService = roleService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [OperationFilter("Obtener")]
        [HttpGet]
        public async Task<IActionResult> GetPagesByRoleAsync()
        {
            return Json(await _roleService.GetPagesByRoleAsync(GetRoleId()));
        }
        
    }
}
