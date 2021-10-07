using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CIT.BusinessLogic.Services;
using CIT.Dtos;
using CIT.BusinessLogic.Contracts;
using CIT.Dtos.Requests;

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
        public async Task<IActionResult> Index(Auth authModel)
        {
            try
            {
                var accountResponse = await _accountService.SignInAsync(authModel.Email, authModel.Password);

                return Json(accountResponse);
            }
            catch(Exception ex)
            {
                ViewBag.Error = ex.Message;
                return View();
            }
        }
    }
}
