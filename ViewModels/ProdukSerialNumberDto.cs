namespace InventoryMultiGudang.ViewModels
{
    public class ProdukSerialNumberDto
    {
        public int Id { get; set; }
        public string SerialNumber { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string NamaGudang { get; set; } = string.Empty;
        public string NamaRak { get; set; } = string.Empty;
    }
}
