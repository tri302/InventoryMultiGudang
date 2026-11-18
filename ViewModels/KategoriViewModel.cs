using System.ComponentModel.DataAnnotations;

namespace InventoryMultiGudang.ViewModels
{
    /// <summary>
    /// ViewModel untuk Form Create dan Edit Kategori
    /// </summary>
    public class KategoriViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Nama Kategori wajib diisi.")]
        [StringLength(100, ErrorMessage = "Nama Kategori maksimal 100 karakter.")]
        [Display(Name = "Nama Kategori")]
        public string NamaKategori { get; set; } = string.Empty;

        [StringLength(255, ErrorMessage = "Deskripsi maksimal 255 karakter.")]
        [Display(Name = "Deskripsi")]
        public string? Deskripsi { get; set; }
    }

    /// <summary>
    /// Model pembungkus respons Pagination Kategori
    /// </summary>
    public class PagedResultKategori<T>
    {
        public List<T> Items { get; set; } = new List<T>();
        public int TotalItems { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalItems / PageSize);
    }
}