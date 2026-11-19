using Humanizer;

namespace InventoryMultiGudang.Models
{

    public class MatriksStokIndexViewModel
    {        
        public List<WarehouseHeaderDto> Warehouses { get; set; } = new List<WarehouseHeaderDto>();
        public List<ProductStockRowDto> Products { get; set; } = new List<ProductStockRowDto>();
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int TotalRecords { get; set; }
        public string RefNo { get; set; }
        public DateTime TanggalCetak { get; set; }
        public string OperatorName { get; set; }
    }

    public class WarehouseHeaderDto
    {
        public int Id { get; set; }
        public string KodeGudang { get; set; }
        public string NamaGudang { get; set; }
    }

    public class ProductStockRowDto
    {
        public int ProductId { get; set; }
        public string Sku { get; set; }
        public string NamaProduk { get; set; }
        public string Satuan { get; set; }
        public int safety_stock { get; set; }

        // Dictionary<GudangId, StockQty> untuk data cross-tab matrix
        public Dictionary<int, int> WarehouseStocks { get; set; } = new Dictionary<int, int>();        

        // Ubah dari integer tunggal menjadi Dictionary<int, int> (Key: GudangId, Value: SafetyStock)
        public Dictionary<int, int> SafetyStockWarehouse { get; set; } = new Dictionary<int, int>();

        // Dictionary<GudangId, MinStock> atau batas minimum stok jika diperlukan
        public Dictionary<int, int> WarehouseMinStocks { get; set; } = new Dictionary<int, int>();
    }
}