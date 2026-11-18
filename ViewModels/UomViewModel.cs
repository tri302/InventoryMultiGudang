using System.ComponentModel.DataAnnotations;

namespace InventoryMultiGudang.ViewModels
{
    public class UomViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Code UOM wajib diisi.")]
        [StringLength(20, ErrorMessage = "Code maksimal 20 karakter.")]
        [Display(Name = "UOM Code")]
        public string Code { get; set; } = string.Empty;

        [Required(ErrorMessage = "Name UOM wajib diisi.")]
        [StringLength(100, ErrorMessage = "Name maksimal 100 karakter.")]
        [Display(Name = "UOM Name")]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "Description")]
        [StringLength(255, ErrorMessage = "Deskripsi maksimal 255 karakter.")]
        public string? Description { get; set; }

        [Display(Name = "Status Aktif")]
        public bool IsActive { get; set; } = true;
    }
}
