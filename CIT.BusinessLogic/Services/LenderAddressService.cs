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
    public class LenderAddressService : ILenderAddressService
    {
        private readonly ILenderAddressRepository _lenderAddressRepository;

        public LenderAddressService(ILenderAddressRepository lenderAddressRepository)
        {
            _lenderAddressRepository = lenderAddressRepository;
        }
        public async Task<LenderAddress> SaveLenderAddressAsync(int lenderBusinessId, int addressId)
        {
            var lenderAddressEntity = new LenderAddress()
            {
                LenderBusinessId = lenderBusinessId,
                AddressId = addressId
            };

            await _lenderAddressRepository.AddAsync(lenderAddressEntity);
            await _lenderAddressRepository.SaveChangesAsync();

            return lenderAddressEntity;
        }
    }
}
