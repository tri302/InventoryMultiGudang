namespace InventoryMultiGudang.ViewModels
{
    public class AssetPerKaryawanViewModel
    {
        public int KaryawanId { get; set; }
        public string Nik { get; set; }
        public string NamaKaryawan { get; set; }
        public string Email { get; set; }
        public string NoTelepon { get; set; }
        public int JumlahAsetAktif { get; set; }
    }

    public class KaryawanAssetDetailViewModel
    {
        public string NamaBarang { get; set; }
        public string Sku { get; set; }
        public string SerialNumber { get; set; }
        public string Kondisi { get; set; }
        public string RakBin { get; set; }
        public string TanggalDiserahkan { get; set; }
    }
}