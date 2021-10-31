using CIT.DataAccess.Models;
using CIT.Dtos.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CIT.BusinessLogic.Contracts
{
    public interface IRolePermissionService
    {
        Task<List<RolePermissionDto>> GetRolePermissionsAsync(int roleId);
        Task<List<RolePermissionDto>> AddRolePermissionsAsync(List<RolePermissionDto> rolePermissions, int roleId);
        Task<RolePermissionDto> AddRolePermissionAsync(RolePermissionDto rolepermission, int roleId);
        Task DeleteRolePermissionAsync(int rolePermissionId);
        Task DeleteRolePermissionsAsync(List<int> rolePermissionsId);
        Task DeleteRolePermissionsByRoleIdAsync(int roleId);
    }
}
