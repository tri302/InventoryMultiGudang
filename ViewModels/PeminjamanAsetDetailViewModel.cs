namespace InventoryMultiGudang.ViewModels
{
    public class PeminjamanAsetDetailViewModel
    {
        public int Id { get; set; }
        public int ProdukItemId { get; set; }
        public string NamaBarang { get; set; }
        public string SerialNumber { get; set; }
        public string LokasiAwal { get; set; }
        public int Kuantitas { get; set; } = 1;
        public string KondisiSerah { get; set; }
        public string? KondisiKembali { get; set; }
        public string Sku { get; set; }

        public string Merek { get; set; }
        public string GudangNama { get; set; }
        public string LokasiRak { get; set; }
    }
}
