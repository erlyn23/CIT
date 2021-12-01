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

        public RoleService(IRoleRepository roleRepository, IEntitiesInfoService entitiesInfoService,
            IRolePermissionService rolePermissionService)
        {
            _roleRepository = roleRepository;
            _entitiesInfoService = entitiesInfoService;
            _rolePermissionService = rolePermissionService;
        }

        public async Task<RoleDto> CreateRoleAsync(RoleDto role, int lenderBusinessId)
        {
            var entityInfo = await _entitiesInfoService.AddEntityInfoAsync();

            var roleEntity = new Role()
            {
                RoleName = role.Role,
                LenderBusinessId = lenderBusinessId,
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
            var role = await GetRoleByIdAsync(roleId);
            if(role != null)
            {
                if (role.Role.Equals("Administrador"))
                    throw new Exception("El rol administrador no puede ser eliminado");

                var entityInfo = await _entitiesInfoService.GetEntityInfoAsync(role.EntityInfo.Id);
                entityInfo.UpdatedAt = DateTime.Now;
                entityInfo.Status = 0;
                await _entitiesInfoService.UpdateEntityInfo(entityInfo);
            }
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

        public async Task<List<RoleDto>> GetRolesAsync(int lenderBusinessId)
        {
            var roles = await _roleRepository.GetAllWithFilterAsync(r => r.LenderBusinessId == lenderBusinessId && !r.RoleName.Equals("Administrador"));
            var rolesDto = roles.Select(r => MapRoleAsync(r).Result).ToList();
            return rolesDto.Where(r => r.EntityInfo.Status != 0).ToList();
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
                roleEntity.RoleName = role.Role;
                _roleRepository.Update(roleEntity);
                var entityInfo = await _entitiesInfoService.GetEntityInfoAsync(roleEntity.EntityInfoId);

                entityInfo.UpdatedAt = DateTime.Now;
                await _entitiesInfoService.UpdateEntityInfo(entityInfo);

                if (role.ToDelete.Count() > 0)
                {
                    foreach(var toDelete in role.ToDelete)
                    {
                        var rolePermission = await _rolePermissionService.GetRolePermissionByRolePageAndOperationId(toDelete.RoleId, toDelete.OperationId, toDelete.PageId);

                        if (rolePermission != null) await _rolePermissionService.DeleteRolePermissionAsync(rolePermission.Id);
                    } 
                }


                if (role.RolePermissions.Count() > 0)
                    await _rolePermissionService.AddRolePermissionsAsync(role.RolePermissions, roleEntity.Id);

            }
            else throw new Exception("Este rol no existe en la base de datos.");

            await _roleRepository.SaveChangesAsync();
            return role;
        }

        public async Task<List<PageDto>> GetPagesByRoleAsync(int roleId)
        {
            var rolePermissions = await _rolePermissionService.GetRolePermissionsAsync(roleId);
            var pages = rolePermissions.GroupBy(p => p.PageId).Select(p => new PageDto()
            {
                PageId = p.Key,
                PageName = p.Where(rp => rp.PageId == p.Key).FirstOrDefault().PageName,
                IconClass = p.Where(rp => rp.PageId == p.Key).FirstOrDefault().IconClass,
                Route = p.Where(rp => rp.PageId == p.Key).FirstOrDefault().Route
            }).ToList();

            return pages;
        }
    }
}
