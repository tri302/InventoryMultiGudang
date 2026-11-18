using System.ComponentModel.DataAnnotations;

namespace InventoryMultiGudang.ViewModels
{
    public class DepartmentCreateViewModel
    {
        [Display(Name = "Kode Departemen")]
        [StringLength(20, ErrorMessage = "{0} maksimal {1} karakter.")]
        public string? Code { get; set; } // Opsional: Auto-generate jika kosong

        [Required(ErrorMessage = "Nama Departemen wajib diisi.")]
        [Display(Name = "Nama Departemen")]
        [StringLength(100, ErrorMessage = "{0} maksimal {1} karakter.")]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "Deskripsi / Catatan")]
        [StringLength(500, ErrorMessage = "{0} maksimal {1} karakter.")]
        public string? Description { get; set; }
    }

    public class DepartmentEditViewModel : DepartmentCreateViewModel
    {
        [Required]
        public int Id { get; set; }
    }

    public class DepartmentListViewModel
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
    }

    public class PagedResultDepartment<T>
    {
        public List<T> Items { get; set; } = new List<T>();
        public int TotalItems { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalItems / PageSize);
        public bool HasPreviousPage => PageNumber > 1;
        public bool HasNextPage => PageNumber < TotalPages;
    }
}
