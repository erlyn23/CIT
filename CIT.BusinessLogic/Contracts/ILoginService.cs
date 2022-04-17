using CIT.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIT.BusinessLogic.Contracts
{
    public interface ILoginService
    {
        Task<Login> SaveLoginAsync(string email, string password);
        Task DeleteLoginAsync(string email);
    }
}
