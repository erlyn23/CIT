using CIT.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CIT.DataAccess.Contracts
{
    public interface ILoanRepository: IGenericRepository<Loan>
    {
        Task<List<Loan>> GetAllWithFilterAndWithRelationsAsync(Expression<Func<Loan, bool>> expression);
        Task<Loan> FirstOrDefaultWithRelationsAsync(Expression<Func<Loan, bool>> expression);

    }
}
