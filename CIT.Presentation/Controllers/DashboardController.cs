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
        private readonly IDashboardService _dahsboardService;
        private readonly TokenCreator _tokenCreator;

        public DashboardController(IRoleService roleService,
            IDashboardService dahsboardService,
            TokenCreator tokenCreator,
            IRolePermissionService rolePermissionService) : base(rolePermissionService, tokenCreator)
        {
            _roleService = roleService;
            _dahsboardService = dahsboardService;
            _tokenCreator = tokenCreator;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult LenderBusinessSelector()
        {
            return View();
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [OperationFilter("Obtener")]
        [ServiceFilter(typeof(AuthFilter))]
        [HttpGet]
        public async Task<IActionResult> GetDashboardDataAsync()
        {
            int userId = _tokenCreator.GetUserId(Request);
            int lenderBusinessId = await _tokenCreator.GetLenderBusinessId(Request);

            if (userId == 0)
                return Json(await _dahsboardService.GetLenderBusinessDashboardAsync(lenderBusinessId));
            else
                return Json(await _dahsboardService.GetUserDashboardAsync(lenderBusinessId, userId));
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [OperationFilter("Obtener")]
        [ServiceFilter(typeof(AuthFilter))]
        [HttpGet]
        public async Task<IActionResult> GetPagesByRoleAsync()
        {
            return Json(await _roleService.GetPagesByRoleAsync(GetRoleId()));
        }
        
    }
}
