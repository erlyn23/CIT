using CIT.BusinessLogic.Contracts;
using CIT.Presentation.Controllers;
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
            try
            {
                if (!_tokenCreator.HasTokenExpired(context.HttpContext.Request))
                {
                    var controller = (BaseCITController)context.Controller;
                    var pageProperty = controller.Page;

                    var decodedToken = _tokenCreator.DecodeToken(context.HttpContext.Request);
                    var operation = context.HttpContext.Request.Headers["Operation"].ToString();
                    var page = context.HttpContext.Request.Headers["Page"].ToString();

                    if (pageProperty.Equals(page))
                    {
                        var roleId = decodedToken.Claims.FirstOrDefault(c => c.Type.Equals("Role")).Value;

                        int.TryParse(roleId, out int roleIntId);
                        var userRole = await _roleService.GetRoleByIdAsync(roleIntId);

                        var permission = userRole.RolePermissions.FirstOrDefault(r => r.OperationName.Equals(operation) && r.PageName.Equals(page));
                        if (permission != null)
                            await next();
                        else
                            context.Result = new BadRequestObjectResult("No tienes permisos para esta operación");
                    }
                    else
                        context.Result = new BadRequestObjectResult("No tienes permisos para esta operación");
                }
                else
                    context.Result = new UnauthorizedResult();
            }
            catch(Exception ex)
            {
                context.Result = new BadRequestObjectResult(ex.ToString());
            }
        }

        
    }
}
