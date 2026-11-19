using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryMultiGudang.Models
{

    [Table("Gudang")]
    public class Gudang
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string KodeGudang { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string NamaGudang { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        public string TipeGudang { get; set; } = string.Empty; // "fisik" / "virtual"

        [Required]
        [StringLength(100)]
        public string PicGudang { get; set; } = string.Empty;

        [StringLength(500)]
        public string? AlamatGudang { get; set; }

        public bool IsActive { get; set; } = true;

        public bool IsAudit { get; set; } = false;

        // Informasi Summary untuk Card UI
        [NotMapped]
        public int TotalProduk { get; set; } = 0;
        [NotMapped]
        public int TotalStokBarang { get; set; } = 0;

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }


        // Gudang Pengirim
        [NotMapped]
        public string GudangAsalNama { get; set; } = string.Empty;
        [NotMapped]
        public string GudangAsalKode { get; set; } = string.Empty;
        [NotMapped]
        public string GudangAsalAlamat { get; set; } = string.Empty;

        // Gudang Penerima
        [NotMapped]
        public string GudangTujuanNama { get; set; } = string.Empty;
        [NotMapped]
        public string GudangTujuanKode { get; set; } = string.Empty;
        [NotMapped]
        public string GudangTujuanAlamat { get; set; } = string.Empty;
    }
}
