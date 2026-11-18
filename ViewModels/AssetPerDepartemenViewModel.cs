namespace InventoryMultiGudang.ViewModels
{
    public class AssetPerDepartemenViewModel
    {
        public int DepartmentId { get; set; }
        public string KodeDept { get; set; }
        public string NamaDepartemen { get; set; }
        public string Deskripsi { get; set; }
        public int JumlahAsetAktif { get; set; }
    }

    public class DepartemenAssetDetailViewModel
    {
        public string NamaBarang { get; set; }
        public string Sku { get; set; }
        public string SerialNumber { get; set; }
        public string Kondisi { get; set; }
        public string RakBin { get; set; }
        public string TanggalDiserahkan { get; set; }
    }
}