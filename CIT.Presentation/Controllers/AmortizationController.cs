using CIT.BusinessLogic.Contracts;
using CIT.Presentation.Filters;
using CIT.Tools;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CIT.Presentation.Controllers
{
    [ServiceFilter(typeof(ExceptionFilter))]
    [PageFilter("Amortizaciones")]
    public class AmortizationController : BaseCITController
    {
        private readonly IAmortizationService _amortizationService;

        public AmortizationController(TokenCreator tokenCreator, IRolePermissionService rolePermissionService, IAmortizationService amortizationService) : base(rolePermissionService, tokenCreator)
        {
            _amortizationService = amortizationService;
        }
        public IActionResult Index()
        {
            return View();
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [OperationFilter("Obtener")]
        [ServiceFilter(typeof(AuthFilter))]
        [HttpGet]
        public async Task<IActionResult> CreateAmortizationAsync(int loanId)
        {
            return Json(await _amortizationService.CreateAmortizationTableAsync(loanId));
        }
    }
}
