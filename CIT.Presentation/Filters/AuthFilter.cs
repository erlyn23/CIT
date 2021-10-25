using CIT.BusinessLogic.Contracts;
using Microsoft.AspNetCore.Http;
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

        public AuthFilter(IRoleService roleService)
        {
            _roleService = roleService;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var decodedToken = DecodeToken(context.HttpContext.Request);
            var operation = context.HttpContext.Request.Headers["Operation"].ToString();
            var page = context.HttpContext.Request.Headers["page"].ToString();


            var roleId = decodedToken.Claims.FirstOrDefault(c => c.Type.Equals("Role")).Value;
            var userRole = await _roleService.GetRoleAsync(int.Parse(roleId));

            var permission = userRole.RolePermissions.FirstOrDefault(r => r.PermissionName.Equals(operation) && r.Page.Equals(page));

            if (permission != null)
                await next();
            else
                throw new Exception("No tienes los permisos para esta operación");
        }

        private JwtSecurityToken DecodeToken(HttpRequest request)
        {
            var bearerHeader = request.Headers["Authorization"];
            var stringToken = bearerHeader.ToString().Replace("Bearer ", "");

            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadJwtToken(stringToken);
            var decodedToken = jsonToken as JwtSecurityToken;

            return decodedToken;
        }
    }
}
