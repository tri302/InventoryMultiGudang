namespace InventoryMultiGudang.ViewModels
{
    public class TransferOrderItemViewModel
    {
        public int ItemId { get; set; }
        public int ProdukId { get; set; }
        public string NamaProduk { get; set; } = string.Empty;
        public string Sku { get; set; } = string.Empty;
        public string SerialNumber { get; set; } = string.Empty;
        public string Satuan { get; set; } = string.Empty;
        public int QtyPermintaan { get; set; }
        public int QtyDiterima { get; set; }
    }
}
