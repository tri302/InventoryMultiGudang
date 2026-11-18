using InventoryMultiGudang.Models;
using InventoryMultiGudang.Models.ViewModels;
using InventoryMultiGudang.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryMultiGudang.Controllers
{
    //[Authorize(Roles = "Super Admin")]
    [Authorize(Policy = "manage roles and permissions")]
    public class RolesPermissionsController : Controller
    {
        private readonly IRolePermissionRepository _repo;

        public RolesPermissionsController(IRolePermissionRepository repo)
        {
            _repo = repo;
        }

        public async Task<IActionResult> Index(string? roleId)
        {
            var roles = await _repo.GetAllRolesAsync();
            var selectedRole = roles.FirstOrDefault(r => r.Id == roleId) ?? roles.FirstOrDefault();

            var permissions = await _repo.GetAllPermissionsAsync();
            var assignedClaims = selectedRole != null
                ? await _repo.GetRoleClaimsAsync(selectedRole.Id)
                : new List<string>();

            var roleItems = roles.Select(r => new RoleItemViewModel
            {
                Id = r.Id,
                Name = r.Name ?? "",
                IsSystemRole = new[] { "Super Admin", "Admin", "Manager", "User" }.Contains(r.Name)
            }).ToList();

            var groupedPermissions = permissions
                .GroupBy(p => p.Category)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(p => new PermissionItemViewModel
                    {
                        Id = p.Id,
                        Name = p.Name,
                        Category = p.Category,
                        Type = p.Type,
                        IsAssigned = assignedClaims.Contains(p.Name)
                    }).ToList()
                );

            var model = new RoleMatrixViewModel
            {
                SelectedRoleId = selectedRole?.Id ?? "",
                SelectedRoleName = selectedRole?.Name ?? "",
                IsSuperAdmin = selectedRole?.Name?.Equals("Super Admin", StringComparison.OrdinalIgnoreCase) ?? false,
                Roles = roleItems,
                GroupedPermissions = groupedPermissions
            };

            return View(model);
        }

        public async Task<IActionResult> PermissionsManager(string? search)
        {
            var data = await _repo.GetPermissionsManagerDataAsync(search);
            ViewBag.Search = search;
            return View(data);
        }

        // --- AJAX ENDPOINTS ---

        [HttpPost]
        public async Task<IActionResult> CreateRole([FromBody] CreateRoleRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.RoleName))
                return Json(new { success = false, message = "Nama Role wajib diisi!" });

            var result = await _repo.CreateRoleAsync(request.RoleName);
            if (result.Succeeded)
                return Json(new { success = true, message = "Role berhasil ditambahkan." });

            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            return Json(new { success = false, message = errors });
        }

        [HttpPost]
        public async Task<IActionResult> CreatePermission([FromBody] CreatePermissionRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.PermissionName))
                return Json(new { success = false, message = "Nama Permission wajib diisi!" });

            var perm = new Permission
            {
                Name = request.PermissionName.Trim().ToLower(),
                Category = string.IsNullOrWhiteSpace(request.Category) ? "General" : request.Category.Trim(),
                Type = string.IsNullOrWhiteSpace(request.Type) ? "Custom" : request.Type.Trim()
            };

            var success = await _repo.CreatePermissionAsync(perm);
            if (success)
                return Json(new { success = true, message = "Permission berhasil ditambahkan." });

            return Json(new { success = false, message = "Gagal menyimpan permission." });
        }

        [HttpPost]
        public async Task<IActionResult> TogglePermission([FromBody] ToggleRolePermissionRequest request)
        {
            if (string.IsNullOrEmpty(request.RoleId) || string.IsNullOrEmpty(request.PermissionName))
                return Json(new { success = false, message = "Data tidak valid." });

            bool success = request.IsChecked
                ? await _repo.AssignPermissionToRoleAsync(request.RoleId, request.PermissionName)
                : await _repo.RemovePermissionFromRoleAsync(request.RoleId, request.PermissionName);

            return Json(new { success, message = success ? "Akses berhasil diperbarui." : "Gagal memperbarui akses." });
        }

        [HttpPost]
        public async Task<IActionResult> DeletePermission(int id)
        {
            var success = await _repo.DeletePermissionAsync(id);
            return Json(new { success, message = success ? "Permission berhasil dihapus." : "Gagal menghapus permission." });
        }
    }
}