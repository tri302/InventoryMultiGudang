using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryMultiGudang.Models
{
    [Table("master_uom")]
    public class UnitOfMeasure
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty; // Contoh: PCS, BOX, CTN
        public string Name { get; set; } = string.Empty; // Contoh: Pieces, Box, Carton
        public string? Description { get; set; }
        public bool IsActive { get; set; } = true;
    }
}