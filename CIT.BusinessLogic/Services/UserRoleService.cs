using CIT.BusinessLogic.Contracts;
using CIT.DataAccess.Contracts;
using CIT.DataAccess.Models;
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
        private readonly IEntitiesInfoService _entityInfoService;

        public UserRoleService(IUserRoleRepository userRoleRepository, IEntitiesInfoService entityInfoService)
        {
            _userRoleRepository = userRoleRepository;
            _entityInfoService = entityInfoService;
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

        public async Task<UserRoleDto> GetUserRoleAsync(int userRoleId)
        {
            var userRole = await _userRoleRepository.FirstOrDefaultAsync(ur => ur.Id == userRoleId);

            var userRoleDto = MapUserRole(userRole);
            return userRoleDto;
        }

        public async Task<List<UserRoleDto>> GetUserRolesAsync()
        {
            var userRoles = await _userRoleRepository.GetAllAsync();
            var userRolesDto = userRoles.Select(ur => MapUserRole(ur)).ToList();
            return userRolesDto;
        }

        public async Task<UserRoleDto> UpdateUserRoleAsync(UserRoleDto userRole)
        {
            var userRoleInDb = await _userRoleRepository.FirstOrDefaultAsync(ur => ur.Id == userRole.UserRoleId);
            userRoleInDb.UserId = userRole.UserId;
            userRoleInDb.RoleId = userRole.RoleId;

            var entityInfo = await _entityInfoService.GetEntityInfoAsync(userRoleInDb.EntityInfoId);
            entityInfo.UpdatedAt = DateTime.Now;
            await _entityInfoService.UpdateEntityInfo(entityInfo);

            _userRoleRepository.Update(userRoleInDb);
            await _userRoleRepository.SaveChangesAsync();
            return userRole;
        }

        private UserRoleDto MapUserRole(Userrole userRole)
        {
            var userRoleDto = new UserRoleDto()
            {
                UserRoleId = userRole.Id,
                RoleId = userRole.RoleId,
                UserId = userRole.UserId
            };

            return userRoleDto;
        }
    }
}
