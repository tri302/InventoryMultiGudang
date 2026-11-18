using System.ComponentModel.DataAnnotations;

namespace InventoryMultiGudang.Models
{
    public class Permission
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [StringLength(100)]
        public string Category { get; set; } = "General";

        [StringLength(50)]
        public string Type { get; set; } = "Custom"; // Custom atau System Core

        public string? Description { get; set; }
    }
}