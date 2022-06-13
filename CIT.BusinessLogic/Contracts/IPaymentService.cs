using CIT.Dtos.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIT.BusinessLogic.Contracts
{
    public interface IPaymentService
    {
        Task<List<PaymentDto>> GetPaymentsAsync(int lenderBusinessId);
        Task<List<PaymentDto>> GetPaymentsByUserIdAsync(int lenderBusinessId, int userId);
        Task<PaymentDto> GetPaymentAsync(int paymentId);
        Task<PaymentDto> AddPaymentAsync(PaymentDto payment, int lenderBusinessId);
        Task<PaymentDto> UpdatePaymentAsync(PaymentDto payment);
        Task DeletePaymentAsync(int paymentId);
        Task DeletePaymentsByLoanAsync(int loanId);
    }
}
