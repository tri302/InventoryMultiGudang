namespace InventoryMultiGudang.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    public enum StatusOpname
    {
        Draft,
        Selesai,
        Dibatalkan
    }

    [Table("StockOpname")]
    public class StockOpname
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(30)]
        public string NoReferensi { get; set; } = string.Empty;

        [Required]
        public int GudangId { get; set; }

        [ForeignKey("GudangId")]
        public virtual Gudang? Gudang { get; set; }

        public string Auditor { get; set; } = "Super Admin";

        public DateTime TanggalMulai { get; set; } = DateTime.Now;

        public DateTime? TanggalSelesai { get; set; }

        public StatusOpname Status { get; set; } = StatusOpname.Draft;

        public string? Catatan { get; set; }

        public virtual ICollection<StockOpnameDetail> Details { get; set; } = new List<StockOpnameDetail>();
        [NotMapped]
        public string? AlasanSelisih { get; set; }
        [NotMapped]
        public string? Keterangan { get; set; }


        [NotMapped]
        public int ProdukId { get; set; }
        [NotMapped]
        public int Selisih { get; set; }
        [NotMapped]
        public DateTime TanggalOpname { get; set; }
        [NotMapped]
        public string? Pembuat { get; set; }        
    }
}
