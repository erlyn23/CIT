using AutoMapper;
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
    public class VehicleAssignmentService : IVehicleAssignmentService
    {
        private readonly IVehicleAssignmentRepository _vehicleAssignmentRepository;
        private readonly IMapper _mapper;

        private const string VEHICLE_ASSIGNED_ERROR = "Este vehículo ya fue asignado o este usuario ya tiene un vehículo, por favor, valida los datos.";

        public VehicleAssignmentService(IVehicleAssignmentRepository vehicleAssignmentRepository, IMapper mapper)
        {
            _vehicleAssignmentRepository = vehicleAssignmentRepository;
            _mapper = mapper;
        }

        public async Task<VehicleAssignmentDto> AssignVehicleAsync(VehicleAssignmentDto vehicleAssignment)
        {
            var isVehicleAssignedTo = await ValidateIfVehicleIsAssignedTo(vehicleAssignment.UserId, vehicleAssignment.VehicleId);

            if (!isVehicleAssignedTo)
            {
                vehicleAssignment.AssignmentDate = DateTime.Now;
                var vehicleAssignmentEntity = _mapper.Map<VehicleAssignment>(vehicleAssignment);

                var savedAssignment = await _vehicleAssignmentRepository.AddAsync(vehicleAssignmentEntity);
                await _vehicleAssignmentRepository.SaveChangesAsync();
                vehicleAssignment.Id = savedAssignment.Id;
                return vehicleAssignment;
            }

            throw new Exception(VEHICLE_ASSIGNED_ERROR);
        }

        public async Task<VehicleAssignmentDto> UpdateAssignmentAsync(VehicleAssignmentDto vehicleAssignment)
        {
            var isVehicleAssigned = await ValidateIfVehicleIsAssignedTo(vehicleAssignment.UserId, vehicleAssignment.VehicleId, vehicleAssignment.Id);

            if (!isVehicleAssigned)
            {
                var assignmentEntity = await _vehicleAssignmentRepository.FirstOrDefaultAsync(v => v.Id == vehicleAssignment.Id);

                if(assignmentEntity != null)
                {
                    assignmentEntity.UserId = vehicleAssignment.UserId;
                    assignmentEntity.VehicleId = vehicleAssignment.VehicleId;
                    assignmentEntity.Comment = vehicleAssignment.Comment;
                    _vehicleAssignmentRepository.Update(assignmentEntity);
                    await _vehicleAssignmentRepository.SaveChangesAsync();
                    return vehicleAssignment;
                }
            }

            throw new Exception(VEHICLE_ASSIGNED_ERROR);
        }

        private async Task<bool> ValidateIfVehicleIsAssignedTo(int userId, int vehicleId, int assignmentId = 0)
        {
            var assignmentInDbByVehicle = await _vehicleAssignmentRepository.FirstOrDefaultAsync(v => v.VehicleId == vehicleId);
            var assignmentInDbByUser = await _vehicleAssignmentRepository.FirstOrDefaultAsync(v => v.UserId == userId);

            if(assignmentId != 0)
            {
                if (assignmentInDbByVehicle != null && assignmentInDbByVehicle.Id != assignmentId)
                    return true;
                else if (assignmentInDbByUser != null && assignmentInDbByUser.Id != assignmentId)
                    return true;
            }
            else
            {
                var isVehicleOrUserAssigned = assignmentInDbByUser != null || assignmentInDbByVehicle != null;

                return isVehicleOrUserAssigned;
            }
            return false;
        }

        public async Task DeleteAssignmentAsync(int assignmentId)
        {
            var vehicleAssignment = await _vehicleAssignmentRepository.FirstOrDefaultAsync(v => v.Id == assignmentId);

            if(vehicleAssignment != null)
            {
                _vehicleAssignmentRepository.Delete(vehicleAssignment);
                await _vehicleAssignmentRepository.SaveChangesAsync();
            }
        }

        public async Task<VehicleAssignmentDto> GetVehicleAssignmentAsync(int vehicleAssignmentId)
        {
            var vehicleAssignment = _mapper.Map<VehicleAssignmentDto>(await _vehicleAssignmentRepository.FirstOrDefaultWithRelationsAsync(v => v.Id == vehicleAssignmentId));

            if (vehicleAssignment != null)
                return vehicleAssignment;

            throw new Exception("Esta asignación no se encuentra o no existe.");
        }

        public async Task<List<VehicleAssignmentDto>> GetVehiclesAssignmentsAsync(int lenderBusinessId)
        {
            var vehicleAssignments = _mapper.Map<List<VehicleAssignmentDto>>(await _vehicleAssignmentRepository.GetVehicleAssignmentsByFilterWithRelationsAsync(v => v.Vehicle.LenderBusinessId == lenderBusinessId));

            return vehicleAssignments;
        }
    }
}
