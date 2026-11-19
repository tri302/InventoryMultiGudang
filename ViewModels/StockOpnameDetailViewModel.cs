using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace InventoryMultiGudang.ViewModels
{
    public class StockOpnameDetailViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Produk wajib dipilih!")]
        public int ProdukId { get; set; }

        public string? NamaProduk { get; set; }
        public string Sku { get; set; } = string.Empty;

        [Required(ErrorMessage = "Stok sistem harus ada.")]
        public int StokSistem { get; set; }

        [Required(ErrorMessage = "Stok fisik wajib diisi.")]
        [Range(0, int.MaxValue, ErrorMessage = "Stok fisik tidak boleh bernilai negatif.")]
        public int StokFisik { get; set; }

        // Properti Selisih dihitung otomatis secara real-time
        public int Selisih => StokFisik - StokSistem;

        public string? AlasanSelisih { get; set; }
        public string? Keterangan { get; set; }
        [ValidateNever]
        public string? Kondisi { get; set; }
        [ValidateNever]
        public List<StockOpnameViewModel>? SnItems { get; set; } = new List<StockOpnameViewModel>();
        [ValidateNever]
        public string? SerialNumber { get; set; }
        [ValidateNever]
        public string? Status { get; set; }
        [ValidateNever]
        public int ProdukItemId { get; set; }
        [ValidateNever]
        public string? TipeBarang { get; set; }
    }
}
