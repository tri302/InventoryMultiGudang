// Models/ViewModels/RolesPermissionsViewModels.cs
namespace InventoryMultiGudang.Models.ViewModels
{
    public class RoleItemViewModel
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public bool IsSystemRole { get; set; }
    }

    public class PermissionItemViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public bool IsAssigned { get; set; }
    }

    public class RoleMatrixViewModel
    {
        public string SelectedRoleId { get; set; } = string.Empty;
        public string SelectedRoleName { get; set; } = string.Empty;
        public bool IsSuperAdmin { get; set; }
        public List<RoleItemViewModel> Roles { get; set; } = new();
        public Dictionary<string, List<PermissionItemViewModel>> GroupedPermissions { get; set; } = new();
    }

    public class PermissionManagerRowViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public List<string> AssignedRoles { get; set; } = new();
    }

    public class CreateRoleRequest
    {
        public string RoleName { get; set; } = string.Empty;
    }

    public class CreatePermissionRequest
    {
        public string PermissionName { get; set; } = string.Empty;
        public string Category { get; set; } = "General";
        public string? Type { get; set; }
    }

    public class ToggleRolePermissionRequest
    {
        public string RoleId { get; set; } = string.Empty;
        public string PermissionName { get; set; } = string.Empty;
        public bool IsChecked { get; set; }
    }
}