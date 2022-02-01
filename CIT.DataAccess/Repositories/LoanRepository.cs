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
    public class LoanRepository : GenericRepository<Loan>, ILoanRepository
    {
        private readonly CentroInversionesTecnocorpDbContext _dbContext;
        public LoanRepository(CentroInversionesTecnocorpDbContext dbContext): base(dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<List<Loan>> GetAllWithFilterAndWithRelationsAsync(Expression<Func<Loan, bool>> expression)=>        
            await _dbContext.Loans.Include(u => u.LenderBusiness).Where(expression).ToListAsync();
        
    }
}
