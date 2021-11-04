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
    public class RolePermissionService : IRolePermissionService
    {
        private readonly IRolePermissionRepository _rolePermissionRepository;
        private readonly IOperationRepository _operationRepository;
        private readonly IPageRepository _pageRepository;

        public RolePermissionService(IRolePermissionRepository rolePermissionRepository, IOperationRepository operationRepository, IPageRepository pageRepository)
        {
            _rolePermissionRepository = rolePermissionRepository;
            _operationRepository = operationRepository;
            _pageRepository = pageRepository;
        }
        public async Task<RolePermissionDto> AddRolePermissionAsync(RolePermissionDto rolePermission, int roleId)
        {
            var rolePermissionEntity = new Rolepermission()
            {
                OperationId = rolePermission.OperationId,
                PageId = rolePermission.PageId,
                RoleId = roleId
            };
            await _rolePermissionRepository.AddAsync(rolePermissionEntity);
            await _rolePermissionRepository.SaveChangesAsync();
            rolePermission.Id = rolePermissionEntity.Id;
            return rolePermission;
        }

        public async Task<List<RolePermissionDto>> AddRolePermissionsAsync(List<RolePermissionDto> rolePermissions, int roleId)
        {
            var rolePermissionsDto = new List<RolePermissionDto>();
            
            foreach (var rolePermission in rolePermissions)
            {
                var rolePermissionInDb = await GetRolePermissionByRolePageAndOperationId(roleId, rolePermission.OperationId, rolePermission.PageId);

                if(rolePermissionInDb == null)
                    rolePermissionsDto.Add(await AddRolePermissionAsync(rolePermission, roleId));
            }
            return rolePermissionsDto;
        }

        public async Task DeleteRolePermissionAsync(int rolePermissionId)
        {
            var rolePermission = await _rolePermissionRepository.FirstOrDefaultAsync(r => r.Id == rolePermissionId);

            if (rolePermission != null)
                _rolePermissionRepository.Delete(rolePermission);

            await _rolePermissionRepository.SaveChangesAsync();
        }

        public async Task DeleteRolePermissionsAsync(List<int> rolePermissionsId)
        {
            foreach(var rolePermissionId in rolePermissionsId)
            {
                await DeleteRolePermissionAsync(rolePermissionId);
            }
        }

        public async Task DeleteRolePermissionsByRoleIdAsync(int roleId)
        {
            var permissions = await _rolePermissionRepository.GetAllWithFilterAsync(r => r.RoleId == roleId);

            foreach (var permission in permissions)
                await DeleteRolePermissionAsync(permission.Id);
        }

        public async Task<RolePermissionDto> GetRolePermissionByRolePageAndOperationId(int roleId, int operationId, int pageId)
        {
            var rolePermission = await _rolePermissionRepository.FirstOrDefaultAsync(rp => rp.RoleId == roleId && rp.OperationId == operationId && rp.PageId == pageId);

            if(rolePermission != null)
            {
                var rolePermissionDto = await MapRolePermissionAsync(rolePermission);
                return rolePermissionDto;
            }
            return null;
        }

        public async Task<List<RolePermissionDto>> GetRolePermissionsAsync(int roleId)
        {
            var rolePermissions = await _rolePermissionRepository.GetAllWithFilterAsync(r => r.RoleId == roleId);
            var rolePermissionsDto = rolePermissions.Select(r => MapRolePermissionAsync(r).Result).ToList();
            return rolePermissionsDto;
        }

        public async Task<RolePermissionDto> MapRolePermissionAsync(Rolepermission rolePermission)
        {
            var pages = await _pageRepository.GetAllAsync();
            var operations = await _operationRepository.GetAllAsync();
            var rolePermissionDto = new RolePermissionDto()
            {
                Id = rolePermission.Id,
                RoleId = rolePermission.RoleId,
                OperationName = operations.Where(o => o.Id == rolePermission.OperationId).FirstOrDefault().OperationName,
                OperationId = operations.Where(o => o.Id == rolePermission.OperationId).FirstOrDefault().Id,
                PageId = pages.Where(p => p.Id == rolePermission.PageId).FirstOrDefault().Id,
                PageName = pages.Where(p => p.Id == rolePermission.PageId).FirstOrDefault().PageName
            };
            return rolePermissionDto;
        }
    }
}
