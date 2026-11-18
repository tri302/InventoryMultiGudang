namespace InventoryMultiGudang.ViewModels
{
    public class ProdukSerialNumbersResultDto
    {
        public string NamaProduk { get; set; } = string.Empty;
        public string Sku { get; set; } = string.Empty;
        public List<ProdukSerialNumberDto> Items { get; set; } = new();
    }
}
