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
        public override void OnException(ExceptionContext context)
        {
            //TODO: Guardar los errores en la tabla de logs y mostrar al usuario solamente que se ha ocurrido un error.
            var stringBuilder = new StringBuilder();
            stringBuilder.AppendLine(context.Exception.Message);
            stringBuilder.AppendLine(context.Exception.StackTrace);
            if (context.Exception.InnerException != null)
            {
                stringBuilder.AppendLine(context.Exception.InnerException.Message);
                stringBuilder.AppendLine(context.Exception.InnerException.StackTrace);
            }

            context.Result = new Microsoft.AspNetCore.Mvc.BadRequestObjectResult(stringBuilder.ToString());
            
        }
    }
}
