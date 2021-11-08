using CIT.BusinessLogic.Contracts;
using CIT.Dtos.Requests;
using CIT.Presentation.Filters;
using CIT.Tools;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CIT.Presentation.Controllers
{
    [ServiceFilter(typeof(ExceptionFilter))]
    public class UsersController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly ILenderBusinessService _lenderBusinessService;
        private readonly TokenCreator _tokenCreator;
        private int _userId;

        public UsersController(IAccountService accountService, ILenderBusinessService lenderBusinessService, TokenCreator tokenCreator)
        {
            _accountService = accountService;
            _lenderBusinessService = lenderBusinessService;
            _tokenCreator = tokenCreator;
        }
        public IActionResult Index()
        {
            return View();
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ServiceFilter(typeof(AuthFilter))]
        [HttpPost]
        public async Task<IActionResult> SaveUserAsync([FromBody] UserDto user)
        {
            var userType = GetUserTypeAndUserId();
            if (ModelState.IsValid)
            {
                int lenderBusinessId;
                if (userType.Equals("User"))
                {
                    var userInDb = await _accountService.GetUserAsync(_userId);
                    lenderBusinessId = userInDb.LenderBusinessId;
                }
                else
                {
                    var lenderBusiness = await _lenderBusinessService.GetLenderBusinessAsync(_userId);
                    lenderBusinessId = lenderBusiness.Id;
                }
                user.LenderBusinessId = lenderBusinessId;
                return Json(await _accountService.RegisterUserAsync(user));
            }
            else
                return Json(ModelState.Values.ToList());
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ServiceFilter(typeof(AuthFilter))]
        [HttpGet]
        public async Task<IActionResult> GetUsersAsync()
        {
            var userType = GetUserTypeAndUserId();
            if (userType.Equals("User"))
            {
                var user = await _accountService.GetUserAsync(_userId);
                return Json(await _accountService.GetUsersAsync(user.LenderBusinessId));
            }
            else
            {
                var lender = await _lenderBusinessService.GetLenderBusinessAsync(_userId);
                return Json(await _accountService.GetUsersAsync(lender.Id));
            }

        }

        private string GetUserTypeAndUserId()
        {
            var decodedToken = _tokenCreator.DecodeToken(Request);
            _userId = int.Parse(decodedToken.Claims.Where(c => c.Type.Equals("nameid")).FirstOrDefault().Value);
            return decodedToken.Claims.Where(c => c.Type.Equals("UserType")).FirstOrDefault().Value;
        }


    }
}
