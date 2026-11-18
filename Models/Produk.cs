using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryMultiGudang.Models
{
    [Table("Produk")]
    public class Produk
    {
        [Key]
        public int Id { get; set; }

        // Tanpa Serial Number (Bulk) 
        // Pelacakan Serial Number (SN) 
        public string PelacakanBarang { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string Sku { get; set; } = string.Empty;

        [Required]
        [MaxLength(200)]
        public string NamaProduk { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string Kategori { get; set; } = string.Empty;

        [MaxLength(100)]
        public string? Merek { get; set; }

        [Required]
        [MaxLength(50)]
        public string Satuan { get; set; } = "Pcs";

        [Required]
        [MaxLength(100)]
        public string TipeBarang { get; set; } = "Inventory (Komersial)";

        [Column(TypeName = "decimal(18,2)")]
        public decimal HargaBeli { get; set; } = 0;

        [Column(TypeName = "decimal(18,2)")]
        public decimal HargaPerolehan { get; set; } = 0;

        public string? Deskripsi { get; set; }

        public int Stok { get; set; } = 0;

        // Relation Foreign Key ke Gudang
        public int GudangId { get; set; }
        public virtual Gudang? Gudang { get; set; }

        // Relation Foreign Key ke Rak
        public int? RakId { get; set; }
        public virtual Rak? Rak { get; set; }

        // Relation Foreign Key ke UnitOfMeasure (UOM)
        public int? MasterUomId { get; set; }
        public virtual UnitOfMeasure? MasterUom { get; set; }
    }
}