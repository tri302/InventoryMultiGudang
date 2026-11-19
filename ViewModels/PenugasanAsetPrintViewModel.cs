using InventoryMultiGudang.Models;

namespace InventoryMultiGudang.ViewModels
{
    public class PenugasanAsetPrintViewModel
    {
        public int Id { get; set; }
        public string KodePenugasan { get; set; } = string.Empty;
        public DateTime TanggalSerahTerima { get; set; }
        public StatusPenugasan Status { get; set; }
        public string? Keterangan { get; set; }

        // Dinamis dari tbl systemsettings
        public string PerusahaanNama { get; set; } = string.Empty;
        public string PerusahaanSlogan { get; set; } = string.Empty;
        public string PerusahaanAlamat { get; set; } = string.Empty;
        public string PerusahaanTelp { get; set; } = string.Empty;
        public string PerusahaanEmail { get; set; } = string.Empty;

        // Pihak Penyerah (Issuer) dari User Login / Default Sistem
        public string AdminPenyerah { get; set; } = "Administrator";

        // Pihak Penerima (Receiver - Departemen) dari DB
        public string DepartemenNama { get; set; } = "-";
        public string DepartemenKode { get; set; } = "-";
        public string DepartemenKeterangan { get; set; } = "-";

        public List<PenugasanAsetPrintDetailViewModel> Details { get; set; } = new();
    }

    public class PenugasanAsetPrintDetailViewModel
    {
        public string Sku { get; set; } = string.Empty;
        public string? Merek { get; set; }
        public string NamaBarang { get; set; } = string.Empty;
        public string GudangNama { get; set; } = string.Empty;
        public string LokasiRak { get; set; } = string.Empty;
        public string SerialNumber { get; set; } = string.Empty;
        public string KondisiSerah { get; set; } = string.Empty;
    }
}