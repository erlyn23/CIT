using CIT.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CIT.DataAccess.Contracts
{
    public interface IRoleRepository : IGenericRepository<Role>
    {
        Task<List<Role>> GetRolesWithRelationsAsync();
        Task<Role> GetRoleWithRelationAsync(Expression<Func<Role, bool>> expression);
    }
}
