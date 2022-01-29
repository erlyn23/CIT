using CIT.Presentation.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CIT.Presentation.Filters
{
    public class OperationFilter : ActionFilterAttribute
    {
        private string _operation;
        public OperationFilter(string operation)
        {
            _operation = operation;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            context.HttpContext.Request.Headers.Add("MethodOperation", _operation);
            base.OnActionExecuting(context);
        }
    }
}
