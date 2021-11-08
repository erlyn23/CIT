using CIT.BusinessLogic.Contracts;
using CIT.DataAccess.Contracts;
using CIT.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIT.BusinessLogic.Services
{
    public class LenderRoleService : ILenderRoleService
    {
        private readonly ILenderRoleRepository _lenderRoleRepository;

        public LenderRoleService(ILenderRoleRepository lenderRoleRepository)
        {
            _lenderRoleRepository = lenderRoleRepository;
        }
        public async Task<LenderRole> SaveLenderRoleAsync(LenderRole lenderRole)
        {
            await _lenderRoleRepository.AddAsync(lenderRole);
            await _lenderRoleRepository.SaveChangesAsync();
            return lenderRole;
        }
    }
}
