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
        Task<RoleDto> CreateRoleAsync(RoleDto role, int lenderBusinessId);
        Task<RoleDto> UpdateRoleAsync(RoleDto role);
        Task<List<RoleDto>> GetRolesAsync();
        Task<RoleDto> GetRoleByIdAsync(int roleId);
        Task<RoleDto> GetRoleByNameAsync(string roleName);
        Task DeleteRoleAsync(int roleId);
    }
}
