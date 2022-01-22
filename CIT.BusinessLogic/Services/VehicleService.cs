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
    public class VehicleService : IVehicleService
    {
        private readonly IVehicleRepository _vehicleRepository;
        private readonly IEntitiesInfoService _entitiesInfoService;

        public VehicleService(IVehicleRepository vehicleRepository, IEntitiesInfoService entitiesInfoService)
        {
            _vehicleRepository = vehicleRepository;
            _entitiesInfoService = entitiesInfoService;
        }
        public async Task<VehicleDto> AddVehicleAsync(VehicleDto vehicle, int lenderBusinessId)
        {
            var entityInfo = await _entitiesInfoService.AddEntityInfoAsync();

            var vehicleEntity = new Vehicle()
            {
                Brand = vehicle.Brand,
                Model = vehicle.Model,
                Enrollment = vehicle.Enrollment.ToString(),
                LicensePlate = vehicle.LicensePlate.ToString(),
                Color = vehicle.Color,
                Year = vehicle.Year,
                LenderBusinessId = lenderBusinessId,
                EntityInfoId = entityInfo.Id
            };

            var savedVehicle = await _vehicleRepository.AddAsync(vehicleEntity);
            await _vehicleRepository.SaveChangesAsync();

            vehicle.VehicleId = savedVehicle.Id;
            return vehicle;
        }
        public async Task DeleteVehicleAsync(int vehicleId)
        {
            var vehicle = await GetVehicleByIdAsync(vehicleId);
            if (vehicle!=null)
            {
                var entityInfo = await _entitiesInfoService.GetEntityInfoAsync(vehicle.EntityInfo.Id);
                entityInfo.UpdatedAt = DateTime.Now;
                entityInfo.Status = 0;
                await _entitiesInfoService.UpdateEntityInfo(entityInfo);
            }
        }
        public async Task<VehicleDto> GetVehicleByIdAsync(int vehicleId)
        {
            var vehicle = await _vehicleRepository.FirstOrDefaultAsync(x=>x.Id==vehicleId);

            VehicleDto vehicleDto = null;
            if (vehicle!=null)
            {
                vehicleDto = await MapVehicleAsync(vehicle);
            }

            if (vehicleDto != null)
            {
                if (vehicleDto.EntityInfo.Status != 0)
                    return vehicleDto;
                else
                    throw new Exception("Este vehículo no existe en la base de datos");
            }
            else
                throw new Exception("Este vehículo no existe en la base de datos");

        }
        public async Task<List<VehicleDto>> GetVehiclesAsync(int lenderBusinessId)
        {
            var vehicles = await _vehicleRepository.GetAllWithFilterAndWithRelationsAsync(u => u.LenderBusinessId == lenderBusinessId);

            var vehiclesDtos = vehicles.Select(u => MapVehicleAsync(u).Result).ToList();
            return vehiclesDtos.Where(u => u.EntityInfo.Status != 0).ToList();
        }
        public async Task<VehicleDto> UpdateVehicleAsync(VehicleDto vehicle)
        {
            var vehicleInDb = await _vehicleRepository.FirstOrDefaultAsync(v=>v.Id==vehicle.VehicleId);
            if (vehicleInDb != null)
            {
                vehicleInDb.Brand = vehicle.Brand;
                vehicleInDb.Model = vehicle.Model;
                vehicleInDb.Enrollment = vehicle.Enrollment.ToString();
                vehicleInDb.LicensePlate = vehicle.LicensePlate.ToString();
                vehicleInDb.Color = vehicle.Color;
                vehicleInDb.Year = vehicle.Year;
                vehicleInDb.LenderBusinessId = vehicle.LenderBusinessId;

                var entityInfo = await _entitiesInfoService.GetEntityInfoAsync(vehicleInDb.EntityInfoId);
                entityInfo.UpdatedAt = DateTime.Now;
                await _entitiesInfoService.UpdateEntityInfo(entityInfo);

                _vehicleRepository.Update(vehicleInDb);
                await _vehicleRepository.SaveChangesAsync();
            }
            return vehicle;
        }
        private async Task<VehicleDto> MapVehicleAsync(Vehicle vehicle)

        {
            var entityinfo = await _entitiesInfoService.GetEntityInfoAsync(vehicle.EntityInfoId);
            var vehicleDto = new VehicleDto()
            {
                VehicleId = vehicle.Id,
                Brand = vehicle.Brand,
                Model = vehicle.Model,
                Enrollment = int.Parse(vehicle.Enrollment),
                LicensePlate = vehicle.LicensePlate,
                Color = vehicle.Color,
                Year = vehicle.Year,
                LenderBusinessId=vehicle.LenderBusinessId,
                EntityInfo = new EntityInfoDto()
                {
                    CreatedAt = entityinfo.CreatedAt,
                    UpdatedAt = entityinfo.UpdatedAt,
                    Status = entityinfo.Status
                }
            };
            return vehicleDto;
        }
    }
}
