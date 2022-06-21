using CIT.Dtos.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIT.BusinessLogic.Contracts
{
    public interface ILoanService
    {

        Task<LoanDto> AddLoanAsync(LoanDto loanDto, int lenderBusinessId);
        Task<LoanDto> UpdateLoanAsync(LoanDto loan);
        Task<List<LoanDto>> GetLoansByLenderBusinessAsync(int lenderBusinessId);
        Task<List<LoanDto>> GetLoansByUserAsync(int lenderBusinessId, int userId);
        Task<List<LoanDto>> GetLoansByNameAsync(int lenderBusinessId, string loanName, int userId = 0);
        Task<LoanDto> GetloanByIdAsync(int loanId);
        Task DeleteLoanAsync(int loanId);
    }
}
