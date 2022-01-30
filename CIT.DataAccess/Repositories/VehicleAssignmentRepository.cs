using CIT.DataAccess.Contracts;
using CIT.DataAccess.DbContexts;
using CIT.DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CIT.DataAccess.Repositories
{
    public class VehicleAssignmentRepository : GenericRepository<VehicleAssignment>, IVehicleAssignmentRepository
    {
        private readonly CentroInversionesTecnocorpDbContext _dbContext;

        public VehicleAssignmentRepository(CentroInversionesTecnocorpDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<VehicleAssignment> FirstOrDefaultWithRelationsAsync(Expression<Func<VehicleAssignment, bool>> expression) =>
            await _dbContext.VehicleAssignments.Include(v => v.User).Include(v => v.Vehicle).Where(expression).FirstOrDefaultAsync();

        public async Task<List<VehicleAssignment>> GetVehicleAssignemntsWithRelationsAsync() =>
            await _dbContext.VehicleAssignments.Include(v => v.User).Include(v => v.Vehicle).ToListAsync();

        public async Task<List<VehicleAssignment>> GetVehicleAssignmentsByFilterWithRelationsAsync(Expression<Func<VehicleAssignment, bool>> expression) =>
            await _dbContext.VehicleAssignments.Include(v => v.User).Include(v => v.Vehicle).Where(expression).ToListAsync();
    }
}
