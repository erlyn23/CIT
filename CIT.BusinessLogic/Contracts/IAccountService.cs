using CIT.Dtos.Requests;
using CIT.Dtos.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIT.BusinessLogic.Contracts
{
    public interface IAccountService
    {
        Task<AccountResponse> SignInAsync(string email, string password, int lenderBusinessId);
        Task<List<LenderBusinessDto>> GetLenderBusinessByUserAsync(string email, string password);
        Task<bool> ActivateAccountAsync(string docId, string verificationNumber);
    }
}
