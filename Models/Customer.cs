using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryMultiGudang.Models
{

    [Table("Customers")]
    public class Customer
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(20)]
        public string KodeCustomer { get; set; } = string.Empty;

        [Required]
        [StringLength(150)]
        public string NamaCustomer { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        public string NoTelepon { get; set; } = string.Empty;

        [Required]
        [StringLength(500)]
        public string Alamat { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;       
    }
}