using CIT.Dtos.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIT.BusinessLogic.Contracts
{
    public interface IAmortizationService
    {
        Task<AmortizationTableDto> CreateAmortizationTableAsync(int loanId);
    }
}
