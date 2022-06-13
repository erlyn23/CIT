using CIT.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CIT.DataAccess.Contracts
{
    public interface IPaymentRepository : IGenericRepository<Payment>
    {
        Task<List<Payment>> GetAllByFilterWithRelationsAsync(Expression<Func<Payment, bool>> expression);
        Task<Payment> FirstOrDefaultWithRelationsAsync(Expression<Func<Payment, bool>> expression);
    }
}
