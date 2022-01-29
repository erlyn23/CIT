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

            int lenderBusinessId = await _tokenCreator.GetLenderBusinessId(context.HttpContext.Request);

            var logDto = new LogDto()
            {
                Operation = context.HttpContext.Request.Headers["Operation"],
                LenderBusinessId = lenderBusinessId,
                ResultMessageOrObject = stringBuilder.ToString(),
                LogDate = DateTime.UtcNow
            };
            await _logService.SaveLogAsync(logDto);

            context.Result = new Microsoft.AspNetCore.Mvc.BadRequestObjectResult($"Ha ocurrido un error interno en el sistema: {context.Exception.Message}, revisa los logs del sistema para más detalles.");
            
        }
    }
}
