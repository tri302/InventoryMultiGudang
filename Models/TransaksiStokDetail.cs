using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryMultiGudang.Models
{
    [Table("TransaksiStokDetail")]
    public class TransaksiStokDetail
    {
        public int Id { get; set; }
        public int TransaksiStokId { get; set; }
        public TransaksiStok? TransaksiStok { get; set; }
        public int ProdukId { get; set; }
        public Produk? Produk { get; set; }
        public int RakId { get; set; }
        public Rak? Rak { get; set; }
        public int Jumlah { get; set; }
        public string? SerialNumbers { get; set; }

        [NotMapped]
        public int Kuantitas { get; set; }
        [NotMapped]
        public int GudangId { get; set; }
        [NotMapped]
        public virtual Gudang? Gudang { get; set; }

        [NotMapped]
        public List<TransaksiStokDetail> Details { get; set; } = new List<TransaksiStokDetail>();
        [NotMapped]
        public DateTime TanggalPosting { get; set; }
        [NotMapped]
        public string TipeTransaksi { get; set; }
        [NotMapped]
        public string NomorReferensi { get; set; }
    }
}
