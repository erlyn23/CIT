using CIT.Dtos.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIT.BusinessLogic.Contracts
{
    public interface IAddressService
    {
        Task<AddressDto> CreateAddressAsync(AddressDto address);
        Task<AddressDto> GetAddressAsync(int addressId);
        Task<AddressDto> UpdateAddressAsync(AddressDto address);

    }
}
