using InventoryMultiGudang.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace InventoryMultiGudang.ViewModels
{
    // ViewModel untuk Entry Formulir Audit
    public class StockOpnameFormViewModel
    {
        public int Id { get; set; }

        public string? NoReferensi { get; set; }

        [Required(ErrorMessage = "Gudang audit wajib diisi!")]
        public int GudangId { get; set; }

        public string? NamaGudang { get; set; }
        public string? KodeGudang { get; set; }

        [StringLength(500, ErrorMessage = "Catatan maksimal 500 karakter.")]
        public string? Catatan { get; set; }

        public List<StockOpnameDetailViewModel> Items { get; set; } = new List<StockOpnameDetailViewModel>();
        [ValidateNever]
        public int? StokFisik { get; set; }
        public int? StokSistem { get; set; }
        public int? Selisih { get; set; }

        [ValidateNever]
        public Produk? Produk { get; set; }
        [ValidateNever]
        public StatusOpname Status { get; set; }
        [ValidateNever]
        public string? Auditor { get; set; } = "Super Admin";
        [ValidateNever]
        public DateTime TanggalMulai { get; set; }
        [ValidateNever]
        public DateTime? TanggalSelesai { get; set; }
        [ValidateNever]
        public string? AlamatGudang { get; set; }
    }
}
