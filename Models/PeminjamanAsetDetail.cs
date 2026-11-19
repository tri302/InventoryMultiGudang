using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryMultiGudang.Models
{
    [Table("peminjamanasetdetail")]
    public class PeminjamanAsetDetail
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int PeminjamanAsetID { get; set; }

        [Required]
        public int ProdukItemId { get; set; }

        [Required]
        [StringLength(150)]
        public string NamaBarang { get; set; }

        [StringLength(100)]
        public string SerialNumber { get; set; }

        [StringLength(100)]
        public string LokasiAwal { get; set; }

        public int Kuantitas { get; set; } = 1;

        [StringLength(50)]
        public string KondisiSerah { get; set; } = "Good";

        [StringLength(50)]
        public string? KondisiKembali { get; set; }

        // Navigasi Balik ke Header
        [ForeignKey("PeminjamanAsetID")]
        public virtual PeminjamanAset PeminjamanAset { get; set; }

        // Navigasi Opsional ke Tabel ProdukItem (jika diperlukan relasi langsung)
        [ForeignKey("ProdukItemId")]
        public virtual ProdukItem ProdukItem { get; set; }


        [NotMapped]
        public string Sku { get; set; }
        [NotMapped]
        public string Merek { get; set; }
        [NotMapped]
        public string GudangNama { get; set; }
        [NotMapped]
        public string LokasiRak { get; set; }
    }
}