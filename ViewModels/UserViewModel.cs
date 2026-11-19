using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace InventoryMultiGudang.Models
{
    public class UserViewModel
    {
        public string? Id { get; set; } // Beri tanda tanya (?) agar opsional saat Create

        [Required(ErrorMessage = "Full Name wajib diisi.")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email wajib diisi.")]
        [EmailAddress(ErrorMessage = "Format email tidak valid.")]
        public string Email { get; set; } = string.Empty;
        [ValidateNever]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public string? Password { get; set; } = string.Empty;
        public List<string> Roles { get; set; } = new();

        // untuk relasi Karyawan
        public int? KaryawanId { get; set; }
        public List<SelectListItem>? KaryawanList { get; set; }
        public string KaryawanText { get; set; }
    }

    public class UserListViewModel
    {
        public List<UserViewModel> Users { get; set; }
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
        public int TotalUsers { get; set; }
    }
}