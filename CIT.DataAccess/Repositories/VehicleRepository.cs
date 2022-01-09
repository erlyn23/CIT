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
    public class VehicleRepository : GenericRepository<Vehicle>, IVehicleRepository
    {
        private readonly CentroInversionesTecnocorpDbContext _dbContext;
        public VehicleRepository(CentroInversionesTecnocorpDbContext dbContext): base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Vehicle>> GetAllWithFilterAndWithRelationsAsync(Expression<Func<Vehicle, bool>> expression)=>
             await _dbContext.Vehicles.Include(u => u.LenderBusiness).Where(expression).ToListAsync();

    }
}
