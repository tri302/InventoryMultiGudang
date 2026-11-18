namespace InventoryMultiGudang.Repository
{
    public class AssetSerialNumberViewModel
    {
        public int Id { get; set; }
        public string SerialNumber { get; set; }
        public string NamaBarang { get; set; }
        public string Sku { get; set; }
        public string Merek { get; set; }
        public string KodeGudang { get; set; }
        public string NamaGudang { get; set; }
        public string NamaRak { get; set; }
        public string Status { get; set; }
    }
}