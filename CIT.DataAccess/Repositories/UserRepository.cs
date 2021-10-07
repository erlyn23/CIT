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
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        private readonly CentroInversiontesTecnocorpDbContext _dbContext;
        public UserRepository(CentroInversiontesTecnocorpDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<User> FirstOrDefaultWithRelationsAsync(Expression<Func<User, bool>> expression) => 
            await _dbContext.Users.Where(expression)
                .Include(u => u.EntityInfo).Include(u => u.Userrole).FirstOrDefaultAsync();
    }
}
