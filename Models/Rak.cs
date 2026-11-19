using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryMultiGudang.Models
{

    [Table("Rak")]
    public class Rak
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(20)]
        public string Area { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string NamaRak { get; set; } = string.Empty;

        [Required]
        [MaxLength(20)]
        public string BinNo { get; set; } = string.Empty;

        [Required]
        [MaxLength(50)]
        public string KodeLokasi { get; set; } = string.Empty;

        [MaxLength(255)]
        public string? Deskripsi { get; set; }

        public bool IsActive { get; set; } = true;
        // Relation Foreign Key ke Gudang
        public int GudangId { get; set; }
        public Gudang? Gudang { get; set; }
    }
}
