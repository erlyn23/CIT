using CIT.Dtos.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIT.BusinessLogic.Contracts
{
    public interface IUserRoleService
    {
        Task<UserRoleDto> AddUserRoleAsync(UserRoleDto userRole);
        Task<UserRoleDto> UpdateUserRoleAsync(UserRoleDto userRole);
        Task<List<UserRoleDto>> GetUserRolesAsync();
        Task<UserRoleDto> GetUserRoleAsync(int userRoleId);
        Task<UserRoleDto> GetUserRoleByUserAndRoleIdAsync(int userId, int roleId);
        Task DeleteUserRoleByRoleIdAsync(int roleId);
        Task DeleteUserRoleAsync(int userRoleId);
    }
}
