using CIT.BusinessLogic.Contracts;
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
    public abstract class BaseCITController : Controller
    {
        private readonly IRolePermissionService _rolePermissionService;
        private readonly TokenCreator _tokenCreator;
        public BaseCITController(IRolePermissionService rolePermissionService, TokenCreator tokenCreator)
        {
            _rolePermissionService = rolePermissionService;
            _tokenCreator = tokenCreator;
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [HttpGet]
        protected async Task<IActionResult> GetPermissionsByPageAndRoleAsync(int pageId)
        {
            return Json(await _rolePermissionService.GetOperationByPageAndRoleAsync(pageId, GetRoleId()));
        }

        protected int GetRoleId()
        {
            var roleClaim = _tokenCreator.DecodeToken(Request).Claims.Where(c => c.Type.Equals("Role")).FirstOrDefault();
            return int.Parse(roleClaim.Value);
        }
    }
}
