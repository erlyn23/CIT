using CIT.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIT.BusinessLogic.Contracts
{
    public interface ILenderAddressService
    {
        Task<LenderAddress> SaveLenderAddressAsync(int lenderBusinessId, int addressId);
    }
}
