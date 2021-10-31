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
        private readonly IEntitiesInfoService _entitiesInfoService;

        public RoleService(IRoleRepository roleRepository, IRolePermissionRepository rolePermissionRepository, IPageRepository pageRepository, IOperationRepository operationRepository, IEntitiesInfoService entitiesInfoService)
        {
            _roleRepository = roleRepository;
            _rolePermissionRepository = rolePermissionRepository;
            _operationRepository = operationRepository;
            _pageRepository = pageRepository;
            _entitiesInfoService = entitiesInfoService;
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

            if (role.RolePermissions.Count() > 0)
                await SaveRolePermissionsAsync(role, roleEntity.Id);

            await _rolePermissionRepository.SaveChangesAsync();

            role.RoleId = savedRole.Id;
            return role;
        }

        private async Task SaveRolePermissionsAsync(RoleDto role, int roleId)
        {
            var rolePermissionsResult = new List<Rolepermission>();

            foreach (var rolePermission in role.RolePermissions)
            {
                var permission = new Rolepermission()
                {
                    OperationId = rolePermission.OperationId,
                    PageId = rolePermission.PageId,
                    RoleId = roleId
                };
                rolePermissionsResult.Add(permission);
            }

            await _rolePermissionRepository.AddRangeAsync(rolePermissionsResult.ToArray());
        }

        public async Task DeleteRoleAsync(int roleId)
        {
            var role = await _roleRepository.FirstOrDefaultAsync(r => r.Id == roleId);
            if(role != null)
            {
                await _entitiesInfoService.DeleteEntityInfoAsync(role.EntityInfoId);
                _roleRepository.Delete(role);
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
            var rolePermissions = await _rolePermissionRepository.GetAllWithFilterAsync(r => r.RoleId == role.Id);
            var pages = await _pageRepository.GetAllAsync();
            var operations = await _operationRepository.GetAllAsync();
            var entityinfo = await _entitiesInfoService.GetEntityInfoAsync(role.EntityInfoId);
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
                }).ToList(),
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

                _entitiesInfoService.UpdateEntityInfo(entityInfo);
            }
            else throw new Exception("Este rol no existe en la base de datos.");

            await _roleRepository.SaveChangesAsync();
            return role;
        }
    }
}
