namespace InventoryMultiGudang.ViewModels
{
    using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
    using System.ComponentModel.DataAnnotations;
    public class RakViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Area / Zone wajib diisi.")]
        [StringLength(20, ErrorMessage = "Area maksimal 20 karakter.")]
        [Display(Name = "AREA / ZONE")]
        public string Area { get; set; } = string.Empty;

        [Required(ErrorMessage = "Nama Rak wajib diisi.")]
        [StringLength(50, ErrorMessage = "Nama Rak maksimal 50 karakter.")]
        [Display(Name = "NAMA RAK")]
        public string NamaRak { get; set; } = string.Empty;

        [Required(ErrorMessage = "Bin No wajib diisi.")]
        [StringLength(20, ErrorMessage = "Bin No maksimal 20 karakter.")]
        [Display(Name = "BIN NO")]
        public string BinNo { get; set; } = string.Empty;

        [Required(ErrorMessage = "Kode Lokasi wajib diisi.")]
        [StringLength(50, ErrorMessage = "Kode Lokasi maksimal 50 karakter.")]
        [RegularExpression(@"^[A-Za-z0-9]+-[A-Za-z0-9]+-[A-Za-z0-9]+$", ErrorMessage = "Format Kode Lokasi harus [Area]-[Rak]-[Bin], misal: A-01-01")]
        [Display(Name = "KODE LOKASI (UNIK)")]
        public string KodeLokasi { get; set; } = string.Empty;

        [StringLength(255, ErrorMessage = "Deskripsi maksimal 255 karakter.")]
        [Display(Name = "DESKRIPSI / CATATAN LOKASI")]
        public string? Deskripsi { get; set; }

        [Display(Name = "Lokasi Aktif")]
        public bool IsActive { get; set; } = true;

        // Properti Tambahan Gudang
        [ValidateNever]
        public int GudangId { get; set; }
        [ValidateNever]
        public string? NamaGudang { get; set; }
    }

    // Class DTO khusus untuk format respons Select2 AJAX
    public class Select2PagedResult
    {
        public object Results { get; set; } = new List<object>();
        public Select2Pagination Pagination { get; set; } = new Select2Pagination();
    }

    public class Select2Pagination
    {
        public bool More { get; set; }
    }

    public class PagedResultRak<T>
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
