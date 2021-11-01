using CIT.BusinessLogic.Contracts;
using CIT.Dtos.Requests;
using CIT.Tools;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace CIT.Presentation.Filters
{
    public class ExceptionFilter : ExceptionFilterAttribute
    {
        private readonly ILogService _logService;
        private readonly TokenCreator _tokenCreator;

        public ExceptionFilter(ILogService logService, TokenCreator tokenCreator)
        {
            _logService = logService;
            _tokenCreator = tokenCreator;
        }

        public override async Task OnExceptionAsync(ExceptionContext context)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine(context.Exception.Message);
            stringBuilder.AppendLine(context.Exception.StackTrace);
            if (context.Exception.InnerException != null)
            {
                stringBuilder.AppendLine(context.Exception.InnerException.Message);
                stringBuilder.AppendLine(context.Exception.InnerException.StackTrace);
            }

            var userId = _tokenCreator.DecodeToken(context.HttpContext.Request).Claims.FirstOrDefault(c => c.Type.ToLower().Equals("nameid")).Value;

            var logDto = new LogDto()
            {
                Operation = context.HttpContext.Request.Headers["Operation"],
                UserId = int.Parse(userId),
                ResultMessageOrObject = stringBuilder.ToString(),
                LogDate = DateTime.UtcNow
            };
            await _logService.SaveLogAsync(logDto);

            context.Result = new Microsoft.AspNetCore.Mvc.BadRequestObjectResult("Ha ocurrido un error interno, revisa los registros de errores en la pantalla de errores del sistema");
            
        }
    }
}
