namespace InventoryMultiGudang.ViewModels
{
    // ViewModel untuk daftar/tabel Stock Opname di Index
    public class StockOpnameListViewModel
    {
        public int Id { get; set; }
        public string NoReferensi { get; set; } = string.Empty;
        public string NamaGudang { get; set; } = string.Empty;
        public string KodeGudang { get; set; } = string.Empty;
        public string Auditor { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string? Catatan { get; set; }
        public DateTime TanggalMulai { get; set; }
        public DateTime? TanggalSelesai { get; set; }
        public string? AlasanSelisih { get; set; }        
        public string? Keterangan { get; set; }
    }        

    // Class penampung Paging & Filter
    public class PagedResultStockOpname<T>
    {
        public List<T> Items { get; set; } = new List<T>();
        public int TotalItems { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalItems / PageSize);
    }
}