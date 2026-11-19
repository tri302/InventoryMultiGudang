using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace InventoryMultiGudang.ViewModels
{
    public class TransaksiStokViewModel
    {
        public int Id { get; set; }

        public bool IsPenjualan { get; set; }

        [Required(ErrorMessage = "Gudang Target wajib dipilih.")]
        public int GudangId { get; set; }

        // Opsional untuk Stock In, Tidak dipakai di Stock Out
        public int? SupplierId { get; set; }

        [Required(ErrorMessage = "Tipe Transaksi wajib diisi.")]
        public string TipeTransaksi { get; set; } = "Stock In";

        public string? NomorReferensi { get; set; }
        public string? Catatan { get; set; }
        public string? SerialNumbers { get; set; }

        // Format Nominal Uang (String input untuk kustom format Rupiah)
        [RegularExpression(@"^[0-9\.\,]+$", ErrorMessage = "Nominal harus berupa angka.")]
        public string? NominalText { get; set; }

        // Validasi opsional gambar (max 2MB)
        public IFormFile? FileGambar { get; set; }

        [Required(ErrorMessage = "Minimal harus menambahkan 1 daftar barang mutasi.")]
        public List<TransaksiStokDetailViewModel> Details { get; set; } = new List<TransaksiStokDetailViewModel>();

        // Dropdown Items
        public List<SelectListItem> OptionGudang { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> OptionSupplier { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> OptionProduk { get; set; } = new List<SelectListItem>();
        public List<SelectListItem> OptionRak { get; set; } = new List<SelectListItem>();
    }
}
