namespace InventoryMultiGudang.ViewModels
{
    public class AssetTimelineViewModel
    {
        public int ProdukItemId { get; set; }
        public string SerialNumber { get; set; }
        public string NamaBarang { get; set; }
        public string Sku { get; set; }
        public string Merek { get; set; }
        public string StatusAktif { get; set; }
        public string NamaGudang { get; set; }
        public string NamaRak { get; set; }
        public string NamaSupplier { get; set; }
        public string TanggalPerolehan { get; set; }
        public decimal HargaPerolehan { get; set; }
        public string QrCodePath { get; set; }
        public List<AssetTimelineItemViewModel> TimelineEvents { get; set; } = new();
    }

    public class AssetTimelineItemViewModel
    {
        public string StatusEvent { get; set; }
        public string Keterangan { get; set; }
        public string Tanggal { get; set; }
        public string Lokasi { get; set; }
    }
}