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
    public class DashboardController : Controller
    {
        private readonly IRoleService _roleService;
        private readonly TokenCreator _tokenCreator;

        public DashboardController(IRoleService roleService, TokenCreator tokenCreator)
        {
            _roleService = roleService;
            _tokenCreator = tokenCreator;
        }

        public IActionResult Index()
        {
            return View();
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet]
        public async Task<IActionResult> GetPagesByRoleAsync()
        {
            var roleClaim = _tokenCreator.DecodeToken(Request).Claims.Where(c => c.Type.Equals("Role")).FirstOrDefault();
            var roleId = int.Parse(roleClaim.Value);
            return Json(await _roleService.GetPagesByRoleAsync(roleId));
        }
        
    }
}
