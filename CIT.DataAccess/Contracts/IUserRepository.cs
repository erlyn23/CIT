using CIT.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CIT.DataAccess.Contracts
{
    public interface IUserRepository : IGenericRepository<User>
    {
        Task<List<User>> GetAllWithRelationsAsync();
        Task<User> FirstOrDefaultWithRelationsAsync(Expression<Func<User, bool>> expression);
    }
}
