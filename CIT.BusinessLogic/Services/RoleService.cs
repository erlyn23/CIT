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
    public class RoleService : IRoleService
    {
        private readonly IRoleRepository _roleRepository;

        public RoleService(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }
        public async Task<RoleDto> CreateRoleAsync(RoleDto role)
        {
            var roleEntity = new Role()
            {
                RoleName = role.Role
            };

            var savedRole = await _roleRepository.AddAsync(roleEntity);
            await _roleRepository.SaveChangesAsync();

            role.RoleId = savedRole.Id;
            return role;
        }

        public async Task DeleteRoleAsync(int roleId)
        {
            var role = await _roleRepository.FirstOrDefaultAsync(r => r.Id == roleId);
            if(role != null)
                _roleRepository.Delete(role);

            await _roleRepository.SaveChangesAsync();
        }

        public async Task<RoleDto> GetRoleAsync(int roleId)
        {
            var role = await _roleRepository.GetRoleWithRelationAsync(roleId);
            var roleDto = new RoleDto()
            {
                RoleId = role.Id,
                Role = role.RoleName
            };
            return roleDto;
        }

        public async Task<List<RoleDto>> GetRolesAsync()
        {
            var roles = await _roleRepository.GetRolesWithRelationsAsync();
            var rolesDto = roles.Select(r => new RoleDto()
            {
                RoleId = r.Id,
                Role = r.RoleName
            }).ToList();
            return rolesDto;
        }

        public async Task<RoleDto> UpdateRoleAsync(RoleDto role)
        {
            var roleEntity = new Role()
            {
                Id = role.RoleId,
                RoleName = role.Role
            };
            _roleRepository.Update(roleEntity);
            await _roleRepository.SaveChangesAsync();
            return role;
        }
    }
}
