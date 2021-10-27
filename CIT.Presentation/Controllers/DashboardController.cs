using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CIT.Presentation.Filters;
using CIT.BusinessLogic.Contracts;
using CIT.BusinessLogic.Services;
using CIT.Dtos.Requests;

namespace CIT.Presentation.Controllers
{
    public class DashboardController : Controller
    {
   
        public DashboardController()
        {

        }

        public IActionResult Index()
        {
            return View();
        }
        
    }
}
