namespace InventoryMultiGudang.ViewModels
{
    public class LaporanBarangMasukKeluarIndexViewModel
    {
        // Ringkasan Kartu Metrik (Summary Cards)
        public int TotalUnitMasuk { get; set; }
        public int TotalUnitKeluar { get; set; }
        public int MutasiBersih { get; set; }

        // Daftar Data Transaksi untuk Tabel Utama
        public List<LaporanBarangItemViewModel> ListTransaksi { get; set; } = new List<LaporanBarangItemViewModel>();
    }

    // ViewModel untuk setiap baris transaksi pada tabel
    public class LaporanBarangItemViewModel
    {
        public long Id { get; set; }
        public string TanggalWaktu { get; set; } = string.Empty;
        public string NoReferensi { get; set; } = string.Empty;
        public string NamaGudang { get; set; } = string.Empty;
        public string TipeTransaksi { get; set; } = string.Empty; // "IN" atau "OUT"
        public string SupplierMitra { get; set; } = string.Empty;
        public int JumlahSku { get; set; }
        public int TotalUnit { get; set; }
        public string Pencatat { get; set; } = string.Empty;
        // Rincian Detail Barang untuk Transaksi IN / OUT
        public List<LaporanBarangDetailItemViewModel> Details { get; set; } = new();
        public string? Catatan { get; set; }
    }

    public class LaporanBarangDetailItemViewModel
    {
        public string Sku { get; set; } = string.Empty;
        public string NamaProduk { get; set; } = string.Empty;
        public int Kuantitas { get; set; }
        public string SatuanJumlah { get; set; } = "Unit";
        public string LokasiRak { get; set; } = string.Empty;
        public string SerialNumbers { get; set; } = "Non-Serial Number";
    }

    // ViewModel untuk dropdown dinamis (Gudang & Produk) dengan AJAX Search
    public class DropdownItemViewModel
    {
        public int Id { get; set; }
        public string Text { get; set; } = string.Empty;
    }
}