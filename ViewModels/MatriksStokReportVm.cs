namespace InventoryMultiGudang.ViewModels
{
    public class MatriksStokReportVm
    {
        public string RefNo { get; set; } = "SR-MX-" + DateTime.Now.ToString("yyyyMMdd");
        public DateTime TanggalCetak { get; set; } = DateTime.Now;
        public string OperatorName { get; set; } = "Super Admin";

        public string SiteName { get; set; }
        public string SiteDescription { get; set; }
        public string CompanyAddress { get; set; }
        public string CompanyPhone { get; set; }
        public string CompanyEmail { get; set; }
        

        // List gudang dinamis dari tabel `gudang`
        public List<GudangHeaderVm> DaftarGudang { get; set; } = new();

        public int TotalVarianProduk { get; set; }
        public int TotalGudangTerpantau { get; set; }
        public int TotalProdukKritis { get; set; }

        public List<MatriksStokRowVm> Rows { get; set; } = new();
        // List untuk header kolom gudang
        public List<WarehouseDto> Warehouses { get; set; }
        // List untuk baris data produk
        public List<ProductStockDto> Products { get; set; }

        public static explicit operator List<object>(MatriksStokReportVm v)
        {
            throw new NotImplementedException();
        }
    }

    public class GudangHeaderVm
    {
        public int GudangId { get; set; }
        public string KodeGudang { get; set; }
        public string NamaGudang { get; set; }
        public string TipeGudang { get; set; }
    }

    public class MatriksStokRowVm
    {
        public string NamaProduk { get; set; }
        public string Sku { get; set; }
        public string Satuan { get; set; }

        // Key: GudangId, Value: Jumlah Stok dari tabel `produk.Stok`
        public Dictionary<int, decimal> StokPerGudang { get; set; } = new();
        public Dictionary<int, int> SafetyStockPerGudang { get; set; } = new Dictionary<int, int>();
    }

}