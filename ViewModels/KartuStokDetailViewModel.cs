
namespace InventoryMultiGudang.ViewModels
{
    public class KartuStokDetailViewModel
    {
        public int GudangId { get; set; }

        public int ProdukId { get; set; }

        public string KodeGudang { get; set; } = string.Empty;

        public string NamaGudang { get; set; } = string.Empty;

        public string Sku { get; set; } = string.Empty;

        public string NamaProduk { get; set; } = string.Empty;

        public string Satuan { get; set; } = string.Empty;

        public KartuStokSummaryViewModel Summary { get; set; } = new();

        public List<KartuStokRowViewModel> Rows { get; set; } = new();

        public PaginationViewModel Pagination { get; set; } = new();
        public DateTime TanggalPosting { get; internal set; }
        public string TipeTransaksi { get; internal set; }
        public string NomorReferensi { get; internal set; }
        public int QtyIn { get; internal set; }
        public decimal QtyOut { get; internal set; }
        public decimal SaldoAkhir { get; internal set; }
        public string Pembuat { get; internal set; }
        public string? Catatan { get; internal set; }
        public string KodeLokasi { get; internal set; }
    }
}
