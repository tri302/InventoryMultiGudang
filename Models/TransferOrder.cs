using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryMultiGudang.Models
{
    public enum StatusTransfer
    {
        Pending,
        DalamPerjalanan,
        Diterima,
        Dibatalkan,
        InTransit,
        Received
    }

    public class TransferOrder
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string NomorDokumen { get; set; } = string.Empty;

        [Required]
        public int GudangAsalId { get; set; }
        public Gudang? GudangAsal { get; set; }

        [Required]
        public int GudangTujuanId { get; set; }
        public Gudang? GudangTujuan { get; set; }

        public string? Catatan { get; set; }
        public StatusTransfer Status { get; set; } = StatusTransfer.Pending;

        public DateTime TanggalDibuat { get; set; } = DateTime.Now;
        public DateTime? TanggalDikirim { get; set; }
        public DateTime? TanggalDiterima { get; set; }

        public string Pembuat { get; set; } = "Super Admin";

        public List<TransferOrderDetail> TransferOrdersDetails { get; set; } = new();

        [NotMapped]
        public int ProdukId { get; set; }

        [NotMapped]
        public int Jumlah { get; set; }
    }    
}
