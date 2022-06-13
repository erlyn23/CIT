using CIT.BusinessLogic.Contracts;
using CIT.Dtos.Requests;
using CIT.Presentation.Filters;
using CIT.Tools;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CIT.Presentation.Controllers
{
    [ServiceFilter(typeof(ExceptionFilter))]
    [PageFilter("Pagos")]
    public class PayController : BaseCITController
    {
        private readonly IPaymentService _paymentService;
        private readonly TokenCreator _tokenCreator;

        public PayController(IPaymentService paymentService, 
            TokenCreator tokenCreator, 
            IRolePermissionService rolePermissionService) : base(rolePermissionService, tokenCreator)
        {
            _paymentService = paymentService;
            _tokenCreator = tokenCreator;
        }
        public IActionResult Index()
        {
            return View();
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [OperationFilter("Obtener")]
        [ServiceFilter(typeof(AuthFilter))]
        [HttpGet]
        public async Task<IActionResult> GetPaymentsAsync()
        {
            var userId = _tokenCreator.GetUserId(Request);
            var lenderBusinessId = await _tokenCreator.GetLenderBusinessId(Request);
            List<PaymentDto> payments;

            if (userId == 0)
                payments = await _paymentService.GetPaymentsAsync(lenderBusinessId);
            else
                payments = await _paymentService.GetPaymentsByUserIdAsync(lenderBusinessId, userId);


            return Json(payments);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [OperationFilter("Agregar")]
        [ServiceFilter(typeof(AuthFilter))]
        [HttpPost]
        public async Task<IActionResult> AddPaymentAsync([FromBody] PaymentDto payment)
        {
            var lenderBusinessId = await _tokenCreator.GetLenderBusinessId(Request);
            if (ModelState.IsValid)
                return Json(await _paymentService.AddPaymentAsync(payment, lenderBusinessId));
            
            return Json(ModelState.Values.Select(v => v.Errors.Select(e => e.ErrorMessage)).ToList());
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [OperationFilter("Modificar")]
        [ServiceFilter(typeof(AuthFilter))]
        [HttpPost]
        public async Task<IActionResult> UpdatePaymentAsync([FromBody] PaymentDto payment)
        {
            if (ModelState.IsValid)
                return Json(await _paymentService.UpdatePaymentAsync(payment));

            return Json(ModelState.Values.Select(v => v.Errors.Select(e => e.ErrorMessage)).ToList());
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [OperationFilter("Eliminar")]
        [ServiceFilter(typeof(AuthFilter))]
        [HttpGet]
        public async Task<IActionResult> DeletePaymentAsync(int id)
        {
            await _paymentService.DeletePaymentAsync(id);
            return Json("Pago eliminado correctamente");
        }
    }
}
