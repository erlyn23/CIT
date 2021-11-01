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
        private readonly IEntitiesInfoService _entitiesInfoService;
        private readonly IRolePermissionService _rolePermissionService;
        private readonly IUserRoleService _userRoleService;

        public RoleService(IRoleRepository roleRepository, IEntitiesInfoService entitiesInfoService,
            IRolePermissionService rolePermissionService, IUserRoleService userRoleService)
        {
            _roleRepository = roleRepository;
            _entitiesInfoService = entitiesInfoService;
            _rolePermissionService = rolePermissionService;
            _userRoleService = userRoleService;
        }

        public async Task<RoleDto> CreateRoleAsync(RoleDto role)
        {
            var entityInfo = new Entitiesinfo()
            {
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                Status = 1
            };

            await _entitiesInfoService.AddEntityInfoAsync(entityInfo);

            var roleEntity = new Role()
            {
                RoleName = role.Role,
                EntityInfoId = entityInfo.Id
            };

            var savedRole = await _roleRepository.AddAsync(roleEntity);

            await _roleRepository.SaveChangesAsync();

            if (role.RolePermissions.Count() > 0)
                await _rolePermissionService.AddRolePermissionsAsync(role.RolePermissions, roleEntity.Id);


            role.RoleId = savedRole.Id;
            return role;
        }

        public async Task DeleteRoleAsync(int roleId)
        {
            var role = await _roleRepository.FirstOrDefaultAsync(r => r.Id == roleId);
            if(role != null)
            {
                if (role.RoleName.Equals("Administrador"))
                    throw new Exception("El rol administrador no puede ser eliminado");

                await _rolePermissionService.DeleteRolePermissionsByRoleIdAsync(roleId);
                await _userRoleService.DeleteUserRoleByRoleIdAsync(roleId);
                _roleRepository.Delete(role);
                await _entitiesInfoService.DeleteEntityInfoAsync(role.EntityInfoId);
            }

            await _roleRepository.SaveChangesAsync();
        }

        public async Task<RoleDto> GetRoleByIdAsync(int roleId)
        {
            var role = await _roleRepository.FirstOrDefaultAsync(r => r.Id == roleId);
            var roleDto = await MapRoleAsync(role);
            return roleDto;
        }

        public async Task<RoleDto> GetRoleByNameAsync(string roleName)
        {
            var role = await _roleRepository.FirstOrDefaultAsync(r => r.RoleName.Equals(roleName));
            var roleDto = (role != null) ? await MapRoleAsync(role) : null;
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
            var rolePermissions = await _rolePermissionService.GetRolePermissionsAsync(role.Id);
            var entityinfo = await _entitiesInfoService.GetEntityInfoAsync(role.EntityInfoId);
            var roleDto = new RoleDto()
            {
                RoleId = role.Id,
                Role = role.RoleName,
                RolePermissions = rolePermissions,
                EntityInfo = new EntityInfoDto()
                {
                    CreatedAt = entityinfo.CreatedAt,
                    UpdatedAt = entityinfo.UpdatedAt,
                    Status = entityinfo.Status
                }
            };
            return roleDto;
        }

        public async Task<RoleDto> UpdateRoleAsync(RoleDto role)
        {
            var roleEntity = await _roleRepository.FirstOrDefaultAsync(r => r.Id == role.RoleId);

            if (roleEntity != null)
            {
                _roleRepository.Update(roleEntity);
                var entityInfo = await _entitiesInfoService.GetEntityInfoAsync(roleEntity.EntityInfoId);

                entityInfo.UpdatedAt = DateTime.Now;
                entityInfo.Status = role.EntityInfo.Status;

                await _entitiesInfoService.UpdateEntityInfo(entityInfo);


            }
            else throw new Exception("Este rol no existe en la base de datos.");

            await _roleRepository.SaveChangesAsync();
            return role;
        }
    }
}
