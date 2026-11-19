using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryMultiGudang.Models
{
    [Table("TransaksiStok")]
    public class TransaksiStok
    {
        [Key]
        public int Id { get; set; }
        public string NomorReferensi { get; set; } = string.Empty;
        public int GudangId { get; set; }
        public Gudang? Gudang { get; set; }
        public int? SupplierId { get; set; }
        public Supplier? Supplier { get; set; }
        public string TipeTransaksi { get; set; } = "Stock In"; // "Stock In" / "Stock Out"
        public string? Catatan { get; set; }
        public string Pembuat { get; set; } = "Admin";
        public DateTime TanggalPosting { get; set; } = DateTime.Now;
        public bool IsPenjualan { get; set; } = false;

        public List<TransaksiStokDetail> Details { get; set; } = new List<TransaksiStokDetail>();

        [NotMapped]
        [ValidateNever]
        public int ProdukId { get; set; }

        [NotMapped]
        [ValidateNever]
        public decimal Jumlah { get; set; }

        [NotMapped]
        [ValidateNever]
        public string? KodeLokasi { get; set; }
        [NotMapped]
        [ValidateNever]
        public DateTime TanggalWaktu { get; set; }
        [NotMapped]
        [ValidateNever]
        public string NoReferensi { get; set; }
        [NotMapped]
        [ValidateNever]
        public string? Pencatat { get; set; }
    }
}
