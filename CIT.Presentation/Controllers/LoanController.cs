using CIT.BusinessLogic.Contracts;
using CIT.Dtos.Requests;
using CIT.Presentation.Filters;
using CIT.Tools;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CIT.Presentation.Controllers
{
    [ServiceFilter(typeof(ExceptionFilter))]
    [PageFilter("Prestamos")]
    public class LoanController : BaseCITController
    {
        private readonly TokenCreator _tokenCreator;
        private readonly ILoanService _loanService;

        public LoanController(TokenCreator tokenCreator, IRolePermissionService rolePermissionService, ILoanService loanService) : base(rolePermissionService, tokenCreator)
        {
            _tokenCreator = tokenCreator;
            _loanService = loanService;
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [OperationFilter("Obtener")]
        [ServiceFilter(typeof(AuthFilter))]
        [HttpGet]
        public async Task<IActionResult> GetLoansAsync()
        {
            var lenderBusinessId = await _tokenCreator.GetLenderBusinessId(Request);
            return Json(await _loanService.GetLoanssAsync(lenderBusinessId));
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [OperationFilter("Agregar")]
        [ServiceFilter(typeof(AuthFilter))]
        [HttpPost]
        public async Task<IActionResult> AddLoanAsync([FromBody] LoanDto loan)
        {
            var lenderBusinessId = await _tokenCreator.GetLenderBusinessId(Request);
            if (ModelState.IsValid)
                return Json(await _loanService.AddLoanAsync(loan,lenderBusinessId));

            return Json(ModelState.Values.Select(v => v.Errors.Select(e => e.ErrorMessage)).ToList());
        }


        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [OperationFilter("Modificar")]
        [ServiceFilter(typeof(AuthFilter))]
        [HttpPost]
        public async Task<IActionResult> UpdateLoanAsync([FromBody] LoanDto loan)
        {
            if (ModelState.IsValid)
                return Json(await _loanService.UpdateLoanAsync(loan));

            return Json(ModelState.Values.Select(v => v.Errors.Select(e => e.ErrorMessage)).ToList());
        }

        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        //[OperationFilter("Eliminar")]
        //[ServiceFilter(typeof(AuthFilter))]
        //[HttpGet]
        //public async Task<IActionResult> DeleteAssignmentAsync(int id)
        //{
        //    await _loanService.DeleteLoanAsync(id);
        //    return Json("Asignación eliminada correctamente");
        //}

    }
}
