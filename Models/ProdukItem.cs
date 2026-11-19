namespace InventoryMultiGudang.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("produk_item")]
    public class ProdukItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int ProdukId { get; set; }

        [Required]
        public int GudangId { get; set; }

        [Required]
        public int RakId { get; set; }

        [Required]
        [StringLength(100)]
        public string SerialNumber { get; set; } = string.Empty;

        [Required]
        [StringLength(30)]
        public string Status { get; set; } = "In Stock"; // 'In Stock', 'Assigned', 'Loaned', 'Repair', 'Lost'

        [StringLength(255)]
        public string? QrCodePath { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Navigation Properties
        [ForeignKey(nameof(ProdukId))]
        public virtual Produk? Produk { get; set; }

        [ForeignKey(nameof(GudangId))]
        public virtual Gudang? Gudang { get; set; }

        [ForeignKey(nameof(RakId))]
        public virtual Rak? Rak { get; set; }

        [NotMapped]
        public string? AlasanSelisih { get; set; }
        [NotMapped]
        public string? Keterangan { get; set; }
    }
}
