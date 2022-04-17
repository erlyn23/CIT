using AutoMapper;
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
        private readonly IMapper _mapper;

        public UserRoleService(IUserRoleRepository userRoleRepository, IEntitiesInfoService entityInfoService, IMapper mapper)
        {
            _userRoleRepository = userRoleRepository;
            _entityInfoService = entityInfoService;
            _mapper = mapper;
        }

        public async Task<UserRoleDto> AddUserRoleAsync(UserRoleDto userRole)
        {
            var userRoleEntity = _mapper.Map<Userrole>(userRole);
            var savedEntityInfo = await _entityInfoService.AddEntityInfoAsync();
            userRoleEntity.EntityInfoId = savedEntityInfo.Id;
            await _userRoleRepository.AddAsync(userRoleEntity);
            await _userRoleRepository.SaveChangesAsync();
            userRole.Id = userRoleEntity.Id;
            return userRole;
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

        public async Task<UserRoleDto> GetUserRoleByUserAndRoleIdAsync(int userId, int roleId)
        {
            var userRoles = await GetUserRolesAsync();
            var userRoleDto = userRoles.Where(ur => ur.UserId == userId && ur.RoleId == roleId).FirstOrDefault();
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
            var userRoleInDb = await _userRoleRepository.FirstOrDefaultAsync(ur => ur.Id == userRole.Id);
            int entityInfoId = userRoleInDb.EntityInfoId;

            if (userRoleInDb != null)
                await DeleteUserRoleAsync(userRole.Id);

            var userRoleEntity = new Userrole
            {
                UserId = userRole.UserId,
                RoleId = userRole.RoleId,
                EntityInfoId = entityInfoId
            };
            await _entityInfoService.UpdateEntityInfo(entityInfoId, 1);

            await _userRoleRepository.AddAsync(userRoleEntity);
            await _userRoleRepository.SaveChangesAsync();
            return userRole;
        }

        private UserRoleDto MapUserRole(Userrole userRole)
        {
            var userRoleDto = new UserRoleDto()
            {
                Id = userRole.Id,
                RoleId = userRole.RoleId,
                UserId = userRole.UserId
            };

            return userRoleDto;
        }
    }
}
