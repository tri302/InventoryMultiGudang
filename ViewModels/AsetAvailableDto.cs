namespace InventoryMultiGudang.ViewModels
{
    public class AsetAvailableDto
    {
        public int Id { get; set; }
        public int GudangId { get; set; }
        public string NamaBarang { get; set; } = string.Empty;
        public string Sku { get; set; } = string.Empty;
        public string SerialNumber { get; set; } = string.Empty;
        public string LokasiAwal { get; set; } = string.Empty;
        public string Rak { get; set; } = string.Empty;
        public string Tipe { get; set; } = string.Empty;
        public int Kuantitas { get; set; } = 1;
        public string KondisiSerah { get; set; } = "Good";
    }
}