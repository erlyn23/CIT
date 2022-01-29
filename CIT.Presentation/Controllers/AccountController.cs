using CIT.BusinessLogic.Contracts;
using CIT.Dtos.Requests;
using CIT.Presentation.Filters;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIT.Presentation.Controllers
{
    public class AccountController : Controller
    {
        private readonly ILenderBusinessService _lenderBusinessService;
        private readonly IAccountService _accountService;

        public AccountController(ILenderBusinessService lenderBusinessService, IAccountService accountService)
        {
            _lenderBusinessService = lenderBusinessService;
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
                var userResponse = await _accountService.SignInAsync(authModel.Email, authModel.Password);
                return Json(userResponse);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] LenderBusinessDto lenderBusiness)
        {
            try
            {
                if (ModelState.IsValid)
                    return Json(await _lenderBusinessService.CreateLenderBusinessAsync(lenderBusiness));
                else
                    return Json(ModelState.Values.Select(v => v.Errors.Select(e => e.ErrorMessage)).ToList());
            }
            catch(Exception ex)
            {
                if(ex.InnerException != null)
                    return BadRequest(ex.InnerException.Message);

                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Activate(string email, string code)
        {
            try
            {
                var isActivated = await _accountService.ActivateAccountAsync(email, code);
                if (isActivated)
                    ViewBag.Success = "Tu cuenta ha sido activada correctamente, puedes iniciar sesión";
            }
            catch(Exception ex)
            {
                if (ex.InnerException != null)
                    ViewBag.FatalError = ex.InnerException.Message;

                ViewBag.Error = ex.Message;
            }
            return View();
        }
    }
}
