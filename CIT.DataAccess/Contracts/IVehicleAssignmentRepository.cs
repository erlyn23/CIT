using CIT.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CIT.DataAccess.Contracts
{
    public interface IVehicleAssignmentRepository : IGenericRepository<VehicleAssignment>
    {
        Task<List<VehicleAssignment>> GetVehicleAssignemntsWithRelationsAsync();
        Task<List<VehicleAssignment>> GetVehicleAssignmentsByFilterWithRelationsAsync(Expression<Func<VehicleAssignment, bool>> expression);
        Task<VehicleAssignment> FirstOrDefaultWithRelationsAsync(Expression<Func<VehicleAssignment, bool>> expression);
    }
}
