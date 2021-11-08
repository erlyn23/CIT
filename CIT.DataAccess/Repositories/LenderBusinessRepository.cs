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
    public class LenderBusinessRepository : GenericRepository<LenderBusiness>,  ILenderBusinessRepository
    {
        private readonly CentroInversionesTecnocorpDbContext _dbContext;
        public LenderBusinessRepository(CentroInversionesTecnocorpDbContext dbContext): base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<LenderBusiness> FirstOrDefaultWithRelationsAsync(Expression<Func<LenderBusiness, bool>> expression) => await _dbContext.LenderBusinesses.Where(expression).Include(l => l.LenderRole).Include(l => l.EntityInfo).Include(l => l.LenderAddress).FirstOrDefaultAsync();

    }
}
