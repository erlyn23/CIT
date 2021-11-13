using CIT.BusinessLogic.Contracts;
using CIT.DataAccess.Contracts;
using CIT.DataAccess.Models;
using CIT.Dtos.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIT.BusinessLogic.Services
{
    public class AddressService : IAddressService
    {
        private readonly IAddressRepository _addressRepository;
        private readonly IEntitiesInfoService _entitiesInfoService;

        public AddressService(IAddressRepository addressRepository, IEntitiesInfoService entitiesInfoService)
        {
            _addressRepository = addressRepository;
            _entitiesInfoService = entitiesInfoService;
        }
        public async Task<AddressDto> CreateAddressAsync(AddressDto address)
        {
            var addressEntity = new Address()
            {
                Id = address.Id,
                City = address.City,
                Country = address.Country,
                Province = address.Province,
                Street1 = address.Street1,
                Street2 = address.Street2,
                HouseNumber = address.HouseNumber,
                Latitude = address.Latitude,
                Longitude = address.Longitude
            };

            var savedEntityInfo = await _entitiesInfoService.AddEntityInfoAsync();

            addressEntity.EntityInfoId = savedEntityInfo.Id;
            await _addressRepository.AddAsync(addressEntity);
            await _addressRepository.SaveChangesAsync();
            address.Id = addressEntity.Id;

            return address;
        }

        public async Task<AddressDto> GetAddressAsync(int addressId)
        {
            var addressEntity = await _addressRepository.FirstOrDefaultAsync(a => a.Id == addressId);
            var addressDto = await MapAddress(addressEntity);
            return addressDto;
        }

        public async Task<AddressDto> UpdateAddressAsync(AddressDto address)
        {
            var addressEntity = await _addressRepository.FirstOrDefaultAsync(a => a.Id == address.Id);
            
            addressEntity.Id = address.Id;
            addressEntity.City = address.City;
            addressEntity.Country = address.Country;
            addressEntity.Province = address.Province;
            addressEntity.Street1 = address.Street1;
            addressEntity.Street2 = address.Street2;
            addressEntity.HouseNumber = address.HouseNumber;
            addressEntity.Latitude = address.Latitude;
            addressEntity.Longitude = address.Longitude;
            addressEntity.EntityInfoId = addressEntity.EntityInfoId;
          

            var entityInfo = await _entitiesInfoService.GetEntityInfoAsync(addressEntity.EntityInfoId);
            entityInfo.UpdatedAt = DateTime.Now;
            await _entitiesInfoService.UpdateEntityInfo(entityInfo);

            _addressRepository.Update(addressEntity);
            await _addressRepository.SaveChangesAsync();

            return address;
        }

        private async Task<AddressDto> MapAddress(Address address)
        {
            var entityInfo = await _entitiesInfoService.GetEntityInfoAsync(address.EntityInfoId);

            var addressDto = new AddressDto()
            {
                Id = address.Id,
                City = address.City,
                Country = address.Country,
                Province = address.Province,
                Street1 = address.Street1,
                Street2 = address.Street2,
                HouseNumber = address.HouseNumber,
                Latitude = address.Latitude,
                Longitude = address.Longitude,
                EntityInfo = new EntityInfoDto()
                {
                    CreatedAt = entityInfo.CreatedAt,
                    UpdatedAt = entityInfo.UpdatedAt,
                    Status = entityInfo.Status,
                    Id = entityInfo.Id
                }
            };

            return addressDto;
        }
    }
}
