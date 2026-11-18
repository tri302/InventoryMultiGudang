using System.Security.Claims;
using InventoryMultiGudang.Models;
using InventoryMultiGudang.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace InventoryMultiGudang.Repository
{
    public class RolePermissionRepository : IRolePermissionRepository
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;

        public RolePermissionRepository(RoleManager<IdentityRole> roleManager, ApplicationDbContext context)
        {
            _roleManager = roleManager;
            _context = context;
        }

        public async Task<List<IdentityRole>> GetAllRolesAsync() =>
            await _roleManager.Roles.OrderBy(r => r.Name).ToListAsync();

        public async Task<IdentityRole?> GetRoleByIdAsync(string roleId) =>
            await _roleManager.FindByIdAsync(roleId);

        public async Task<IdentityResult> CreateRoleAsync(string roleName) =>
            await _roleManager.CreateAsync(new IdentityRole(roleName.Trim()));

        public async Task<IdentityResult> DeleteRoleAsync(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null) return IdentityResult.Failed(new IdentityError { Description = "Role tidak ditemukan" });
            return await _roleManager.DeleteAsync(role);
        }

        public async Task<List<Permission>> GetAllPermissionsAsync() =>
            await _context.Set<Permission>().ToListAsync();

        public async Task<Permission?> GetPermissionByIdAsync(int id) =>
            await _context.Set<Permission>().FindAsync(id);

        public async Task<bool> CreatePermissionAsync(Permission permission)
        {
            // Pastikan kategori tidak kosong, jika kosong set default "General"
            if (string.IsNullOrWhiteSpace(permission.Category))
            {
                permission.Category = "General";
            }
            _context.Set<Permission>().Add(permission);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdatePermissionAsync(Permission permission)
        {
            _context.Set<Permission>().Update(permission);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeletePermissionAsync(int id)
        {
            var item = await _context.Set<Permission>().FindAsync(id);
            if (item == null) return false;
            _context.Set<Permission>().Remove(item);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<List<string>> GetRoleClaimsAsync(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null) return new List<string>();

            var claims = await _roleManager.GetClaimsAsync(role);
            return claims.Where(c => c.Type == "Permission").Select(c => c.Value).ToList();
        }

        public async Task<bool> AssignPermissionToRoleAsync(string roleId, string permissionName)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null) return false;

            var claims = await _roleManager.GetClaimsAsync(role);
            if (!claims.Any(c => c.Type == "Permission" && c.Value == permissionName))
            {
                var result = await _roleManager.AddClaimAsync(role, new Claim("Permission", permissionName));
                return result.Succeeded;
            }
            return true;
        }

        public async Task<bool> RemovePermissionFromRoleAsync(string roleId, string permissionName)
        {
            var role = await _roleManager.FindByIdAsync(roleId);
            if (role == null) return false;

            var claims = await _roleManager.GetClaimsAsync(role);
            var targetClaim = claims.FirstOrDefault(c => c.Type == "Permission" && c.Value == permissionName);
            if (targetClaim != null)
            {
                var result = await _roleManager.RemoveClaimAsync(role, targetClaim);
                return result.Succeeded;
            }
            return true;
        }

        public async Task<List<PermissionManagerRowViewModel>> GetPermissionsManagerDataAsync(string? search = null)
        {
            var permissionsQuery = _context.Set<Permission>().AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.ToLower();
                permissionsQuery = permissionsQuery.Where(p =>
                    p.Name.ToLower().Contains(search) ||
                    p.Category.ToLower().Contains(search));
            }

            var permissions = await permissionsQuery.ToListAsync();
            var roles = await _roleManager.Roles.ToListAsync();

            // Direct SQL/Linq Query to fetch role claims mapping
            var roleClaims = await _context.RoleClaims
                .Where(rc => rc.ClaimType == "Permission")
                .ToListAsync();

            var result = new List<PermissionManagerRowViewModel>();

            foreach (var perm in permissions)
            {
                var assignedRoleIds = roleClaims
                    .Where(rc => rc.ClaimValue == perm.Name)
                    .Select(rc => rc.RoleId)
                    .Distinct();

                var roleNames = roles
                    .Where(r => assignedRoleIds.Contains(r.Id))
                    .Select(r => r.Name ?? string.Empty)
                    .ToList();

                // Pastikan "Super Admin" selalu muncul di setiap baris permission
                if (!roleNames.Contains("Super Admin") && roles.Any(r => r.Name == "Super Admin"))
                {
                    roleNames.Insert(0, "Super Admin");
                }

                result.Add(new PermissionManagerRowViewModel
                {
                    Id = perm.Id,
                    Name = perm.Name,
                    Category = perm.Category,
                    Type = perm.Type,
                    AssignedRoles = roleNames
                });
            }

            return result;
        }
    }
}