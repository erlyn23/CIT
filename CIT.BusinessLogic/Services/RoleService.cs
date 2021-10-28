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
        private readonly IRolePermissionRepository _rolePermissionRepository;
        private readonly IOperationRepository _operationRepository;
        private readonly IPageRepository _pageRepository;

        public RoleService(IRoleRepository roleRepository, IRolePermissionRepository rolePermissionRepository, IPageRepository pageRepository, IOperationRepository operationRepository)
        {
            _roleRepository = roleRepository;
            _rolePermissionRepository = rolePermissionRepository;
            _operationRepository = operationRepository;
            _pageRepository = pageRepository;
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

        public async Task DeleteRoleAsync(string roleId)
        {
            var role = await _roleRepository.FirstOrDefaultAsync(r => r.Id.Equals(roleId));
            if(role != null)
                _roleRepository.Delete(role);

            await _roleRepository.SaveChangesAsync();
        }

        public async Task<RoleDto> GetRoleByIdAsync(string roleId)
        {
            var role = await _roleRepository.FirstOrDefaultAsync(r => r.Id.Equals(roleId));
            var roleDto = await MapRoleAsync(role);
            return roleDto;
        }

        public async Task<RoleDto> GetRoleByNameAsync(string roleName)
        {
            var role = await _roleRepository.FirstOrDefaultAsync(r => r.RoleName.Equals(roleName));
            var roleDto = await MapRoleAsync(role);
            return roleDto;
        }

        public async Task<List<RoleDto>> GetRolesAsync()
        {
            var roles = await _roleRepository.GetAllAsync();
            var rolesDto = roles.Select(r => MapRoleAsync(r).Result).ToList();
            return rolesDto;
        }

        private async Task<RoleDto> MapRoleAsync(Role role)
        {
            var rolePermissions = await _rolePermissionRepository.GetAllWithFilterAsync(r => r.RoleId.Equals(role.Id));
            var pages = await _pageRepository.GetAllAsync();
            var operations = await _operationRepository.GetAllAsync();
            var roleDto = new RoleDto()
            {
                RoleId = role.Id,
                Role = role.RoleName,
                RolePermissions = rolePermissions.Select(r => new RolePermissionDto() 
                { 
                    Id = r.Id,
                    RoleId = r.RoleId,
                    OperationName = operations.Where(o => o.Id.Equals(r.OperationId)).FirstOrDefault().OperationName,
                    OperationId = operations.Where(o => o.Id.Equals(r.OperationId)).FirstOrDefault().Id,
                    PageId = pages.Where(p => p.Id.Equals(r.PageId)).FirstOrDefault().Id,
                    PageName = pages.Where(p => p.Id.Equals(r.PageId)).FirstOrDefault().PageName
                }).ToList()
            };
            return roleDto;
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
