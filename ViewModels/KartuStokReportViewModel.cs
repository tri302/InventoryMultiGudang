namespace InventoryMultiGudang.ViewModels
{
    /// <summary>
    /// ViewModel utama untuk halaman laporan/cetak kartu stok.
    /// Digunakan sebagai jembatan antara Repository/Database dengan ReportKartuStok.cshtml.
    /// </summary>
    public class KartuStokReportViewModel
    {
        // ==============================
        // INFORMASI PERUSAHAAN
        // ==============================

        public string NamaPerusahaan { get; set; } = "PT. CONTOH JAYA BERSAMA";

        public string DeskripsiPerusahaan { get; set; } =
            "INVENTORY MULTI-GUDANG MODERN, SINKRONISASI STOK ANTAR-CABANG, PANTAU VALUASI ASET";

        public string AlamatPerusahaan { get; set; } =
            "Komp. Perkantoran Indah Blok B-12, Jakarta Selatan, Indonesia";

        public string TeleponPerusahaan { get; set; } =
            "(021) 555-9830";

        public string EmailPerusahaan { get; set; } =
            "support@contoh.com";


        // ==============================
        // HEADER REPORT
        // ==============================

        public string NomorReport { get; set; } = string.Empty;

        public DateTime TanggalCetak { get; set; }

        public string Operator { get; set; } = string.Empty;


        // ==============================
        // GUDANG
        // ==============================

        public int GudangId { get; set; }

        public string KodeGudang { get; set; } = string.Empty;

        public string NamaGudang { get; set; } = string.Empty;


        // ==============================
        // PRODUK
        // ==============================

        public int ProdukId { get; set; }

        public string Sku { get; set; } = string.Empty;

        public string NamaProduk { get; set; } = string.Empty;

        public string Satuan { get; set; } = string.Empty;


        // ==============================
        // SUMMARY
        // ==============================

        public decimal StokAwal { get; set; }

        public decimal TotalMasuk { get; set; }

        public decimal TotalKeluar { get; set; }

        public decimal StokAkhir { get; set; }


        // ==============================
        // DETAIL MUTASI
        // ==============================

        public List<KartuStokReportRowViewModel> Rows { get; set; } = new();
    }


    /// <summary>
    /// ViewModel satu baris histori mutasi stok pada laporan.
    /// </summary>
    public class KartuStokReportRowViewModel
    {
        public DateTime TanggalPosting { get; set; }

        public string TipeTransaksi { get; set; } = string.Empty;

        public decimal Jumlah { get; set; }

        public decimal StokSebelum { get; set; }

        public decimal StokSesudah { get; set; }

        public string NomorReferensi { get; set; } = string.Empty;

        public string KodeLokasi { get; set; } = string.Empty;

        public string Pembuat { get; set; } = string.Empty;

        public string Catatan { get; set; } = string.Empty;

        // Digunakan untuk menentukan style badge pada UI.
        public string BadgeClass { get; set; } = string.Empty;
    }
}