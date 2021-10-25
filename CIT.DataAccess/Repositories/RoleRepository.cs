using CIT.DataAccess.Contracts;
using CIT.DataAccess.DbContexts;
using CIT.DataAccess.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIT.DataAccess.Repositories
{
    public class RoleRepository : GenericRepository<Role>, IRoleRepository
    {
        private readonly CentroInversionesTecnocorpDbContext _dbContext;

        public RoleRepository(CentroInversionesTecnocorpDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Role>> GetRolesWithRelationsAsync()
        {
            return await _dbContext.Roles.Include(r => r.Rolepermissions).ToListAsync();
        }

        public async Task<Role> GetRoleWithRelationAsync(int roleId)
        {
            return await _dbContext.Roles.Include(r => r.Rolepermissions).FirstOrDefaultAsync(r => r.Id == roleId);
        }
    }
}
