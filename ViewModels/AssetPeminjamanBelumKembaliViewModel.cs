namespace InventoryMultiGudang.ViewModels
{
    public class AssetPeminjamanBelumKembaliViewModel
    {
        public int Id { get; set; }
        public string KodePinjam { get; set; }
        public string NamaPeminjam { get; set; }
        public string NikPeminjam { get; set; }
        public string EmailPeminjam { get; set; }
        public string NamaBarang { get; set; }
        public string SerialNumber { get; set; }
        public DateTime TanggalPinjam { get; set; }
        public DateTime TanggalJatuhTempo { get; set; }
        public string StatusPeminjaman { get; set; } // Misal: "Terlambat (Overdue)" atau "Dipinjam"
        public bool IsOverdue { get; set; }
    }
}