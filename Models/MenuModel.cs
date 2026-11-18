using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace InventoryMultiGudang.Models
{
    // Entitas Database / Model Utama
    public class MenuModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string MenuTitle { get; set; } = string.Empty;

        [Required]
        [StringLength(255)]
        public string Url { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string GroupName { get; set; } = string.Empty;

        [StringLength(100)]
        public string? ParentMenu { get; set; }

        public int SortOrder { get; set; } = 1;

        [StringLength(50)]
        public string IconClass { get; set; } = "bi-question-circle-fill";

        public string? RequireRoles { get; set; } // Disimpan sebagai string terpisah koma (e.g., "Admin,Manager")
        public string? RequirePermissions { get; set; } // Disimpan sebagai string terpisah koma
    }

    // ViewModel untuk keperluan Form Tambah/Ubah dengan Validasi
    public class MenuViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Menu Title wajib diisi.")]
        [StringLength(100, ErrorMessage = "Maksimal panjang 100 karakter.")]
        public string MenuTitle { get; set; } = string.Empty;

        [Required(ErrorMessage = "Path / URL wajib diisi.")]
        [StringLength(255, ErrorMessage = "Maksimal panjang 255 karakter.")]
        public string Url { get; set; } = string.Empty;

        [Required(ErrorMessage = "Group Name wajib dipilih.")]
        public string GroupName { get; set; } = string.Empty;

        public string? ParentMenu { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Sort Order minimal 1.")]
        public int SortOrder { get; set; } = 1;

        public string IconClass { get; set; } = "bi-question-circle-fill";

        public List<string> SelectedRoles { get; set; } = new List<string>();
        public List<string> SelectedPermissions { get; set; } = new List<string>();
    }

    // ViewModel untuk Manajemen Group Menu
    public class MenuGroupViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Nama Group wajib diisi.")]
        [StringLength(100)]
        public string OldGroupName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Nama Group Baru wajib diisi.")]
        [StringLength(100)]
        public string NewGroupName { get; set; } = string.Empty;
    }
}