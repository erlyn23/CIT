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
        private readonly CentroInversionesTecnocorpDbContext _dbContext;
        public UserRepository(CentroInversionesTecnocorpDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<User>> GetAllWithRelationsAsync()=>
            await _dbContext.Users.Include(u => u.EntityInfo).Include(u => u.Userrole).Include(u => u.Useraddress).ToListAsync();
        
        public async Task<User> FirstOrDefaultWithRelationsAsync(Expression<Func<User, bool>> expression) => 
            await _dbContext.Users.Where(expression)
                .Include(u => u.EntityInfo).Include(u => u.Userrole).FirstOrDefaultAsync();

        public async Task<List<User>> GetAllWithFilterAndWithRelationsAsync(Expression<Func<User, bool>> expression) =>
            await _dbContext.Users.Include(u => u.EntityInfo).Include(u => u.Userrole).Include(u => u.Useraddress).Where(expression).ToListAsync();
    }
}
