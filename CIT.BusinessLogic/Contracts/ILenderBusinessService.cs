using CIT.Dtos.Requests;
using CIT.Dtos.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIT.BusinessLogic.Contracts
{
    public interface ILenderBusinessService
    {
        Task<AccountResponse> CreateLenderBusinessAsync(LenderBusinessDto lenderBusiness);
        Task<LenderBusinessDto> GetLenderBusinessAsync(int lenderBusinessId);
    }
}
