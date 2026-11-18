using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace InventoryMultiGudang.Models
{
    public class PenugasanAsetDetail
    {
        [Key]
        public int Id { get; set; }

        public int PenugasanAsetId { get; set; }
        [ForeignKey("PenugasanAsetId")]
        public virtual PenugasanAset? PenugasanAset { get; set; }

        [Required, StringLength(100)]
        public string NamaBarang { get; set; } = string.Empty;

        [StringLength(100)]
        public string? SerialNumber { get; set; }

        [StringLength(100)]
        public string? LokasiAwal { get; set; }

        public int Kuantitas { get; set; } = 1;

        [StringLength(50)]
        public string KondisiSerah { get; set; } = "Good";

        [StringLength(50)]
        public string? KondisiKembali { get; set; }

        [NotMapped]
        public string Sku { get; set; }
        [NotMapped]
        public string Merek { get; set; }
        [NotMapped]
        public string GudangNama { get; set; }
        [NotMapped]
        public string LokasiRak { get; set; }
        [NotMapped]
        public int ProdukItemId { get; set; }
        [NotMapped]
        public int ProdukId { get; set; }
    }
}
