using CIT.BusinessLogic.Contracts;
using CIT.DataAccess.Contracts;
using CIT.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIT.BusinessLogic.Services
{
    public class UserAddressService : IUserAddressService
    {
        private readonly IUserAddressRepository _addressRepository;

        public UserAddressService(IUserAddressRepository addressRepository)
        {
            _addressRepository = addressRepository;
        }
        public async Task<Useraddress> CreateUserAddress(int userId, int addressId)
        {
            var userAddress = new Useraddress()
            {
                UserId = userId,
                AddressId = addressId
            };

            await _addressRepository.AddAsync(userAddress);
            await _addressRepository.SaveChangesAsync();

            return userAddress; 
        }
    }
}
