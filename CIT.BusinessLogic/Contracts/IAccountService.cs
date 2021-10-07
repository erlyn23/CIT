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
        Task<AccountResponse> SignInAsync(string email, string password);
    }
}
