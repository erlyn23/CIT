using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CIT.Presentation.Filters
{
    public class PageFilter : ActionFilterAttribute
    {
        private string _page;
        public PageFilter(string page)
        {
            _page = page;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            context.HttpContext.Request.Headers.Add("PageController", _page);
            base.OnActionExecuting(context);
        }
    }
}
