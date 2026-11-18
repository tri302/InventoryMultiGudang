namespace InventoryMultiGudang.ViewModels
{
    public class AsetAssignedOptionDto
    {
        public int ProdukItemId { get; set; }
        public string DisplayText { get; set; } = string.Empty;
    }

    public class AsetAssignedDetailDto
    {
        public int ProdukItemId { get; set; }
        public string NamaBarang { get; set; } = string.Empty;
        public string Sku { get; set; } = string.Empty;
        public string Brand { get; set; } = string.Empty;
        public string GudangAsal { get; set; } = string.Empty;
        public string SerialNumber { get; set; } = string.Empty;
        public string PemegangSaatIni { get; set; } = string.Empty;
        public string TipePemegangStr { get; set; } = string.Empty;
        public string StatusAsetStr { get; set; } = string.Empty;
        public int TipePemegang { get; set; }
    }

    public class TransferAssetRequestDto
    {
        public int ProdukItemId { get; set; }
        public int TipePenerima { get; set; }
        public int TargetPenerimaId { get; set; }
        public DateTime TanggalTransfer { get; set; }
        public string? Catatan { get; set; }
    }
}