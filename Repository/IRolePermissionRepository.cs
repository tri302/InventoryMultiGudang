using InventoryMultiGudang.Models;
using InventoryMultiGudang.Models.ViewModels;
using Microsoft.AspNetCore.Identity;

namespace InventoryMultiGudang.Repository
{
    public interface IRolePermissionRepository
    {
        Task<List<IdentityRole>> GetAllRolesAsync();
        Task<IdentityRole?> GetRoleByIdAsync(string roleId);
        Task<IdentityResult> CreateRoleAsync(string roleName);
        Task<IdentityResult> DeleteRoleAsync(string roleId);

        Task<List<Permission>> GetAllPermissionsAsync();
        Task<Permission?> GetPermissionByIdAsync(int id);
        Task<bool> CreatePermissionAsync(Permission permission);
        Task<bool> UpdatePermissionAsync(Permission permission);
        Task<bool> DeletePermissionAsync(int id);

        Task<List<string>> GetRoleClaimsAsync(string roleId);
        Task<bool> AssignPermissionToRoleAsync(string roleId, string permissionName);
        Task<bool> RemovePermissionFromRoleAsync(string roleId, string permissionName);
        Task<List<PermissionManagerRowViewModel>> GetPermissionsManagerDataAsync(string? search = null);
    }
}