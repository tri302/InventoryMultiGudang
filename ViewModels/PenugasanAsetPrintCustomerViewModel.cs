using InventoryMultiGudang.Models;

namespace InventoryMultiGudang.ViewModels
{
    public class PenugasanAsetPrintCustomerViewModel
    {
        public int Id { get; set; }
        public string KodePenugasan { get; set; } = string.Empty;
        public DateTime TanggalSerahTerima { get; set; }
        public StatusPenugasan Status { get; set; }
        public string? Keterangan { get; set; }

        // Header Dinamis dari tbl systemsettings
        public string PerusahaanNama { get; set; } = string.Empty;
        public string PerusahaanSlogan { get; set; } = string.Empty;
        public string PerusahaanAlamat { get; set; } = string.Empty;
        public string PerusahaanTelp { get; set; } = string.Empty;
        public string PerusahaanEmail { get; set; } = string.Empty;

        // Pihak Penyerah (Issuer) dari User Login / Default Sistem
        public string AdminPenyerah { get; set; } = "Administrator";

        // Pihak Penerima (Receiver - Customer) dari DB
        public string CustomerNama { get; set; } = "-";
        public string CustomerKode { get; set; } = "-";
        public string CustomerAlamat { get; set; } = "-";
        public string CustomerEmail { get; set; } = "-";
        public string CustomerTelp { get; set; } = "-";

        public List<PenugasanAsetPrintDetailViewModel> Details { get; set; } = new();
    }
}