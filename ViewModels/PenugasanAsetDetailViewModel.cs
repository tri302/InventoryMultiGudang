using InventoryMultiGudang.Models;
using System.ComponentModel.DataAnnotations;

namespace InventoryMultiGudang.ViewModels
{
    public class PenugasanAsetDetailViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Nama Barang wajib diisi.")]
        public string NamaBarang { get; set; } = string.Empty;

        public string? SerialNumber { get; set; }
        public string? LokasiAwal { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Kuantitas minimal 1.")]
        public int Kuantitas { get; set; } = 1;

        [Required(ErrorMessage = "Kondisi serah wajib diisi.")]
        public string KondisiSerah { get; set; } = "Good";

        public string? KondisiKembali { get; set; }
    }

    

    // ViewModel Pagination & Filter Index
    public class PenugasanAsetIndexViewModel
    {
        public int Id { get; set; }
        public string KodePenugasan { get; set; } = string.Empty;
        public string NamaTarget { get; set; } = string.Empty;
        public string TipeTargetStr { get; set; } = string.Empty;
        public DateTime TanggalSerahTerima { get; set; }
        public DateTime? TanggalPengembalian { get; set; }
        public StatusPenugasan Status { get; set; }
        public string Keterangan {  get; set; }
    }

    public class PagedResultPenugasanAset<T>
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