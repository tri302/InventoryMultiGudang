using System.ComponentModel.DataAnnotations;

namespace InventoryMultiGudang.ViewModels
{
    public class TransaksiStokDetailViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Produk wajib dipilih.")]
        public int ProdukId { get; set; }

        [Required(ErrorMessage = "Lokasi/Rak wajib dipilih.")]
        public int RakId { get; set; }

        [Required(ErrorMessage = "Jumlah wajib diisi.")]
        [Range(1, int.MaxValue, ErrorMessage = "Jumlah minimal harus 1.")]
        public int Jumlah { get; set; } = 1;

        public string? SerialNumbers { get; set; }
    }
}
