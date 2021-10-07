using CIT.BusinessLogic.Contracts;
using CIT.Dtos.Requests;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CIT.Presentation.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index([FromBody] Auth authModel)
        {
            try
            {
                return Json(await _accountService.SignInAsync(authModel.Email, authModel.Password));
            }
            catch(Exception ex)
            {
                return Json(ex.Message);
            }
        }
    }
}
