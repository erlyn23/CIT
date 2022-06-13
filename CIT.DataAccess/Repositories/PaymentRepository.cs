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
    public class PaymentRepository : GenericRepository<Payment>, IPaymentRepository
    {
        private readonly CentroInversionesTecnocorpDbContext _dbContext;

        public PaymentRepository(CentroInversionesTecnocorpDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Payment>> GetAllByFilterWithRelationsAsync(Expression<Func<Payment, bool>> expression) =>
            await _dbContext.Payments
            .Include(p => p.EntityInfo)
            .Include(p => p.Loan)
            .Include(p => p.User)
            .Include(p => p.LenderBusiness)
            .Where(expression).ToListAsync();
        public async Task<Payment> FirstOrDefaultWithRelationsAsync(Expression<Func<Payment, bool>> expression) =>
            await _dbContext.Payments
            .Include(p => p.EntityInfo)
            .Include(p => p.Loan)
            .Include(p => p.User)
            .Include(p => p.LenderBusiness)
            .Where(expression).FirstOrDefaultAsync();
    }
}
