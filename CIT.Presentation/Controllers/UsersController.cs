using CIT.Presentation.Filters;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CIT.Presentation.Controllers
{
    public class UsersController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        [ServiceFilter(typeof(AuthFilter))]
        [HttpGet]
        public IActionResult GetNumbers()
        {
            int[] numbers = { 10, 20, 30, 40, 50 };
            return Json(numbers.ToList());
        }
    }
}
