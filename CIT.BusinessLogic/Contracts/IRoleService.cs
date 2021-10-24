using CIT.Dtos.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIT.BusinessLogic.Contracts
{
    public interface IRoleService
    {
        Task<RoleDto> CreateRoleAsync(RoleDto role);
        Task<RoleDto> UpdateRoleAsync(RoleDto role);
        Task<List<RoleDto>> GetRolesAsync();
        Task<RoleDto> GetRoleAsync(int roleId);
        Task DeleteRoleAsync(int roleId);
    }
}
