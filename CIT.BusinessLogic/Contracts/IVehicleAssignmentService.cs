using CIT.Dtos.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIT.BusinessLogic.Contracts
{
    public interface IVehicleAssignmentService
    {
        Task<List<VehicleAssignmentDto>> GetVehiclesAssignmentsAsync(int lenderBusinessId);
        Task<VehicleAssignmentDto> GetVehicleAssignmentAsync(int vehicleAssignmentId);
        Task<VehicleAssignmentDto> GetVehicleAssignmentByUserAsync(int lenderBusinessId, int userId);
        Task<VehicleAssignmentDto> AssignVehicleAsync(VehicleAssignmentDto vehicleAssignment);
        Task<VehicleAssignmentDto> UpdateAssignmentAsync(VehicleAssignmentDto vehicleAssignment);
        Task DeleteAssignmentAsync(int assignmentId);
    }
}
