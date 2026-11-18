namespace InventoryMultiGudang.ViewModels
{
    public class MatriksStokIndexViewModel
    {
        // List untuk header kolom gudang
        public List<WarehouseDto> Warehouses { get; set; }

        // List untuk baris data produk
        public List<ProductStockDto> Products { get; set; }
        public int TotalCount { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int? SelectedGudangId { get; set; }
        public object ListGudang { get; set; }
    }

    // Merepresentasikan data gudang untuk header tabel
    public class WarehouseDto
    {
        public int Id { get; set; }
        public string KodeGudang { get; set; }
        public string NamaGudang { get; set; }
    }

    // Merepresentasikan data produk dan stoknya
    public class ProductStockDto
    {
        public string NamaProduk { get; set; }
        public string Sku { get; set; }
        public string Satuan { get; set; }

        // Menggunakan Dictionary agar mudah diakses berdasarkan Warehouse.Id
        // Key: WarehouseId, Value: Jumlah Stok
        public Dictionary<int, int> WarehouseStocks { get; set; }
    }
}
