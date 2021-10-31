using CIT.BusinessLogic.Contracts;
using CIT.DataAccess.Contracts;
using CIT.Dtos.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIT.BusinessLogic.Services
{
    public class UserRoleService : IUserRoleService
    {
        private readonly IUserRoleRepository _userRoleRepository;

        public UserRoleService(IUserRoleRepository userRoleRepository)
        {
            _userRoleRepository = userRoleRepository;
        }
        public Task<UserRoleDto> AddUserRoleAsync(UserRoleDto userRole)
        {
            throw new NotImplementedException();
        }

        public async Task DeleteUserRoleAsync(int userRoleId)
        {
            var userRole = await _userRoleRepository.FirstOrDefaultAsync(u => u.Id == userRoleId);

            if (userRole != null) _userRoleRepository.Delete(userRole);

            await _userRoleRepository.SaveChangesAsync();
        }

        public async Task DeleteUserRoleByRoleIdAsync(int roleId)
        {
            var userRoles = await _userRoleRepository.GetAllWithFilterAsync(u => u.RoleId == roleId);

            if (userRoles != null)
            {
                foreach(var userRole in userRoles)
                {
                    _userRoleRepository.Delete(userRole);
                }
            }

            await _userRoleRepository.SaveChangesAsync();
        }

        public Task<UserRoleDto> GetUserRoleAsync(int userRole)
        {
            throw new NotImplementedException();
        }

        public Task<List<UserRoleDto>> GetUserRolesAsync()
        {
            throw new NotImplementedException();
        }

        public Task<UserRoleDto> UpdateUserRoleAsync(UserRoleDto userRole)
        {
            throw new NotImplementedException();
        }
    }
}
