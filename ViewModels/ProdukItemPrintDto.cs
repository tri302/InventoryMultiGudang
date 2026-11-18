namespace InventoryMultiGudang.ViewModels
{
    public class ProdukItemPrintDto
    {
        public int Id { get; set; }
        public string SerialNumber { get; set; } = string.Empty;
        public string QrCodePath { get; set; } = string.Empty;
        public string NamaProduk { get; set; } = string.Empty;
        public string KodeGudang { get; set; } = string.Empty;
        public string KodeRak { get; set; } = string.Empty;
    }
}
