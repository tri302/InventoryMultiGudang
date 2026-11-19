namespace InventoryMultiGudang.ViewModels
{
    public class StockOpnameListDto
    {
        public int Id { get; set; }
        public DateTime TanggalPosting { get; set; }
        public string NoReferensi { get; set; }
        public string Gudang { get; set; }
        public int JumlahSku { get; set; }
        public decimal SelisihLebih { get; set; }
        public decimal SelisihKurang { get; set; }
        public decimal SelisihBersih { get; set; }
        public string Pencatat { get; set; }
        public int Status { get; set; } // 0 = Draft / Pending, 1 = Selesai
    }
}