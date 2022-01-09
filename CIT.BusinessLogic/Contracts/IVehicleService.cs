using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CIT.Dtos.Requests;

namespace CIT.BusinessLogic.Contracts
{
    public interface IVehicleService
    {
        Task<VehicleDto> AddVehicleAsync(VehicleDto vehicleDto, int lenderBusinessId);
        Task<VehicleDto> UpdateVehicleAsync(VehicleDto vehicle);
        Task<List<VehicleDto>> GetVehiclesAsync(int lenderBusinessId);
        Task<VehicleDto> GetVehicleByIdAsync(int vehicleId);
        Task DeleteVehicleAsync(int vehicleId);
    }
}
