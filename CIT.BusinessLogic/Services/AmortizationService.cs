using CIT.BusinessLogic.Contracts;
using CIT.Dtos.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIT.BusinessLogic.Services
{
    public class AmortizationService : IAmortizationService
    {
        private readonly ILoanService _loanService;

        public AmortizationService(ILoanService loanService)
        {
            _loanService = loanService;
        }
        public async Task<AmortizationTableDto> CreateAmortizationTableAsync(int loanId)
        {
            var loan = await _loanService.GetloanByIdAsync(loanId);

            if(loan != null)
            {
                var totalLoan = loan.TotalLoan;
                var dues = loan.DuesQuantity;
                var interest = loan.InterestRate;
                var mensualPay = loan.MensualPay;

                var amortizationDto = new AmortizationTableDto();

                for(int i = 0; i <= dues; i++)
                {
                    if(i == 0)
                    {
                        amortizationDto.Periods.Add(i);
                        amortizationDto.Dues.Add(i);
                        amortizationDto.Interests.Add(i);
                        amortizationDto.CapitalPayments.Add(i);
                        amortizationDto.Balance.Add(totalLoan);
                    }
                    else
                    {
                        amortizationDto.Periods.Add(i);

                        amortizationDto.Dues.Add(Math.Round(mensualPay, 2));

                        var actualInterest = amortizationDto.Balance[i - 1] * interest;
                        amortizationDto.Interests.Add(Math.Round(actualInterest, 2));

                        var actualCapitalPayment = mensualPay - actualInterest;
                        amortizationDto.CapitalPayments.Add(Math.Round(actualCapitalPayment, 2));

                        var actualBalance = amortizationDto.Balance[i - 1] - actualCapitalPayment;
                        amortizationDto.Balance.Add(Math.Round(actualBalance, 2));
                    }
                }

                return amortizationDto;
            }

            throw new Exception("Este préstamo no se encuentra en la base de datos, por favor, seleccione uno diferente");
        }
    }
}
