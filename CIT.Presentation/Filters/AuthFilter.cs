using CIT.BusinessLogic.Contracts;
using CIT.Tools;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;

namespace CIT.Presentation.Filters
{
    public class AuthFilter : IAsyncActionFilter
    {
        private readonly IRoleService _roleService;
        private readonly TokenCreator _tokenCreator;

        public AuthFilter(IRoleService roleService, TokenCreator tokenCreator)
        {
            _roleService = roleService;
            _tokenCreator = tokenCreator;
        }


        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var decodedToken = _tokenCreator.DecodeToken(context.HttpContext.Request);
            var operation = context.HttpContext.Request.Headers["Operation"].ToString();
            var page = context.HttpContext.Request.Headers["Page"].ToString();


            var roleId = decodedToken.Claims.FirstOrDefault(c => c.Type.Equals("Role")).Value;
            var userRole = await _roleService.GetRoleByIdAsync(roleId);

            var permission = userRole.RolePermissions.FirstOrDefault(r => r.PermissionName.Equals(operation) && r.Page.Equals(page));

            if (permission != null)
                await next();
            else
                context.Result = new BadRequestObjectResult("No tienes permisos para esta operación");
        }

        
    }
}
