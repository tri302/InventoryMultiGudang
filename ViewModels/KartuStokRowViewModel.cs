namespace InventoryMultiGudang.ViewModels
{
    public class KartuStokRowViewModel
    {
        public int Id { get; set; }

        public DateTime TanggalPosting { get; set; }

        public string TipeTransaksi { get; set; } = string.Empty;

        public int Jumlah { get; set; }

        public int StokSebelum { get; set; }

        public int StokSesudah { get; set; }

        public string NomorReferensi { get; set; } = string.Empty;

        public string Pembuat { get; set; } = string.Empty;

        public string Catatan { get; set; } = string.Empty;

        public string KodeLokasi { get; set; } = string.Empty;

        public string? Supplier { get; set; }

        public bool IsStockIn { get; set; }

        public bool IsStockOut { get; set; }

        public string BadgeClass { get; set; } = "badge-proses";

        public string BadgeIcon { get; set; } = "bi-arrow-left-right";
        public int QtyIn { get; set; }
        public int QtyOut { get; set; }
        public int SaldoAkhir { get; set; }
    }
}
