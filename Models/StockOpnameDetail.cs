using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace InventoryMultiGudang.Models
{
    [Table("StockOpnameDetail")]
    public class StockOpnameDetail
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int StockOpnameId { get; set; }

        [ForeignKey("StockOpnameId")]
        public virtual StockOpname? StockOpname { get; set; }

        [Required]
        public int ProdukId { get; set; }

        [ForeignKey("ProdukId")]
        public virtual Produk? Produk { get; set; }

        public int? ProdukItemId { get; set; }

        [ForeignKey("ProdukItemId")]
        public virtual ProdukItem? ProdukItem { get; set; }

        public int StokSistem { get; set; }

        public int StokFisik { get; set; }

        public int Selisih => StokFisik - StokSistem;

        public string? AlasanSelisih { get; set; }

        public string? Keterangan { get; set; }
        public string? Kondisi { get; set; }
        public string? SerialNumber { get; set; }        

        [NotMapped]
        public object Sku { get; set; }
        [NotMapped]
        public object NamaProduk { get; set; }
        [NotMapped]
        public object Kategori { get; set; }
    }
}
