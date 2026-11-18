namespace InventoryMultiGudang.ViewModels
{
    using System.ComponentModel.DataAnnotations;
    public class ProdukSettingViewModel
    {
        [Required]
        public int ProductId { get; set; }

        [Required]
        public int WarehouseId { get; set; }

        [Required(ErrorMessage = "Safety stock wajib diisi.")]
        [Range(0, int.MaxValue, ErrorMessage = "Nilai safety stock tidak boleh negatif.")]
        public int SafetyStock { get; set; }

        // Properti tambahan opsional untuk keperluan informasi di modal UI
        public string? NamaProduk { get; set; }
        public string? Sku { get; set; }
        public string? NamaGudang { get; set; }
    }
}
