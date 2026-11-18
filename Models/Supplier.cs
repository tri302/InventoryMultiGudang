using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryMultiGudang.Models
{

    [Table("Suppliers")]
    public class Supplier
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string KodeSupplier { get; set; }

        [Required]
        [StringLength(150)]
        public string NamaSupplier { get; set; }

        [StringLength(100)]
        public string Email { get; set; }

        [StringLength(30)]
        public string Telepon { get; set; }

        [StringLength(100)]
        public string NamaPic { get; set; }

        [StringLength(30)]
        public string WhatsappPic { get; set; }

        public string Alamat { get; set; }

        public string Catatan { get; set; }
        
        public bool IsAktif { get; set; } = true;
    }
}
