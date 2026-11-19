using System.ComponentModel.DataAnnotations;
namespace InventoryMultiGudang.ViewModels
{
    public class GudangViewModel
    {
        public int Id { get; set; }

        // [Required(ErrorMessage = "Kode gudang wajib diisi.")]
        // [RegularExpression(@"^GDG-[A-Z0-9]+-\d+$", ErrorMessage = "Format Kode Gudang harus berupa GDG-KOTA-NOMOR (contoh: GDG-JKT-01).")]
        // [Display(Name = "Kode Gudang")]
        public string? KodeGudang { get; set; } = string.Empty;

        [Required(ErrorMessage = "Nama gudang wajib diisi.")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Nama gudang minimal 3 karakter dan maksimal 100 karakter.")]
        [Display(Name = "Nama Gudang")]
        public string NamaGudang { get; set; } = string.Empty;

        [Required(ErrorMessage = "Tipe gudang wajib dipilih.")]
        [Display(Name = "Tipe Gudang")]
        public string TipeGudang { get; set; } = string.Empty;

        [Required(ErrorMessage = "Nama Penanggung Jawab (PIC) wajib diisi.")]
        [StringLength(100, ErrorMessage = "Nama PIC maksimal 100 karakter.")]
        [Display(Name = "Nama PIC")]
        public string PicGudang { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "Alamat maksimal 500 karakter.")]
        [Display(Name = "Alamat Gudang")]
        public string? AlamatGudang { get; set; }

        [Display(Name = "Gudang Aktif")]
        public bool IsActive { get; set; } = true;

        public int TotalProduk { get; set; }
        public int TotalStokBarang { get; set; }
    }

    public class PagedResultGudang<T>
    {
        public List<T> Items { get; set; } = new List<T>();
        public int TotalItems { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalItems / PageSize);
        public bool HasPreviousPage => PageIndex > 1;
        public bool HasNextPage => PageIndex < TotalPages;
    }
}