using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryMultiGudang.Models
{
    [Table("produk_settings")]
    public class ProdukSetting
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Column("produk_id")]
        public int ProdukId { get; set; }

        [Required]
        [Column("gudang_id")]
        public int GudangId { get; set; }

        [Required]
        [Column("safety_stock")]
        public int SafetyStock { get; set; } = 10; // Default nilai minimum stok

        // Navigasi opsional jika relasi diperlukan di EF Core
        [ForeignKey("ProdukId")]
        public virtual Produk? Produk { get; set; }

        [ForeignKey("GudangId")]
        public virtual Gudang? Gudang { get; set; }        
    }
}
