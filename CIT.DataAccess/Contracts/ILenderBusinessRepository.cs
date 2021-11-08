using CIT.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CIT.DataAccess.Contracts
{
    public interface ILenderBusinessRepository : IGenericRepository<LenderBusiness>
    {
        Task<LenderBusiness> FirstOrDefaultWithRelationsAsync(Expression<Func<LenderBusiness, bool>> expression);
    }
}
