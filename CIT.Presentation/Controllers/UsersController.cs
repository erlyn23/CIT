﻿using CIT.BusinessLogic.Contracts;
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
    public class UsersController : BaseCITController
    {
        private readonly IUserService _userService;
        private readonly TokenCreator _tokenCreator;

        public UsersController(IUserService userService, TokenCreator tokenCreator, IRolePermissionService rolePermissionService) : base(rolePermissionService, tokenCreator)
        {
            _userService = userService;
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
            if (ModelState.IsValid)
            {

                int lenderBusinessId = await _tokenCreator.GetLenderBusinessId(Request);
                user.LenderBusinessId = lenderBusinessId;
                return Json(await _userService.RegisterUserAsync(user));
            }
            else
                return Json(ModelState.Values.ToList());
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ServiceFilter(typeof(AuthFilter))]
        [HttpGet]
        public async Task<IActionResult> GetUsersAsync()
        {
            var lenderBusinessId = await _tokenCreator.GetLenderBusinessId(Request);
            return Json(await _userService.GetUsersAsync(lenderBusinessId));

        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ServiceFilter(typeof(AuthFilter))]
        [HttpPost]
        public async Task<IActionResult> UpdateUserAsync([FromBody] UserDto user)
        {
            return Json(await _userService.UpdateUserAsync(user));
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [ServiceFilter(typeof(AuthFilter))]
        [HttpGet]
        public async Task<IActionResult> DeleteUserAsync(int id)
        {
            await _userService.DeleteUserAsync(id);
            return Json("Usuario eliminado correctamente");
        }
    }
}
