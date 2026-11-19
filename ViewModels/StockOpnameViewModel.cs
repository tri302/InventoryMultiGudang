namespace InventoryMultiGudang.ViewModels
{
    public class StockOpnameViewModel
    {
        public int? Id { get; set; }        
        public string? Sku { get; set; } = string.Empty;        
        public string Kondisi { get; set; } = "Ditemukan (OK)";
        public string? SerialNumber { get; set; } = string.Empty;
        public int? GudangId { get; set; }
        public string? Status { get; set; }
        public int ProdukItemId { get; set; }
        public string? AlasanSelisih { get; set; }
        public string? Keterangan { get; set; }
    }
}
