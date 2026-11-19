namespace InventoryMultiGudang.ViewModels
{
    public class AssetPerCustomerViewModel
    {
        public int CustomerId { get; set; }
        public string KodeCustomer { get; set; }
        public string NamaCustomer { get; set; }
        public string Email { get; set; }
        public string NoTelepon { get; set; }
        public int JumlahAsetAktif { get; set; }
    }

    public class CustomerAssetDetailViewModel
    {
        public string NamaBarang { get; set; }
        public string Sku { get; set; }
        public string SerialNumber { get; set; }
        public string Kondisi { get; set; }
        public string RakBin { get; set; }
        public string TanggalDiserahkan { get; set; }
        public string? LokasiAwal { get; set; }
    }
}