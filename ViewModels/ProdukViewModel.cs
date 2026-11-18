using System.ComponentModel.DataAnnotations;

namespace InventoryMultiGudang.ViewModels
{
    public class ProdukViewModel
    {
        public int Id { get; set; }

        // Tanpa Serial Number (Bulk) 
        // Pelacakan Serial Number (SN) 
        [Required(ErrorMessage = "Pelacakan Barang wajib diisi.")]        
        public string PelacakanBarang { get; set; } = string.Empty;

        // [Required(ErrorMessage = "SKU wajib diisi.")]
        // [StringLength(50, ErrorMessage = "SKU maksimal 50 karakter.")]
        public string Sku { get; set; } = string.Empty;

        [Required(ErrorMessage = "Nama produk wajib diisi.")]
        [StringLength(200, ErrorMessage = "Nama produk maksimal 200 karakter.")]
        public string NamaProduk { get; set; } = string.Empty;

        [Required(ErrorMessage = "Kategori wajib dipilih atau diisi.")]
        [StringLength(100, ErrorMessage = "Kategori maksimal 100 karakter.")]
        public string Kategori { get; set; } = string.Empty;

        [StringLength(100, ErrorMessage = "Merek maksimal 100 karakter.")]
        public string Merek { get; set; }

        [Required(ErrorMessage = "Satuan (UoM) wajib dipilih.")]
        public string Satuan { get; set; } = "Pcs";

        [Required(ErrorMessage = "Tipe barang wajib dipilih.")]
        public string TipeBarang { get; set; } = "Inventory (Komersial)";

        [Range(0, double.MaxValue, ErrorMessage = "Harga beli tidak boleh negatif.")]
        public decimal HargaBeli { get; set; }

        [Range(0, double.MaxValue, ErrorMessage = "Harga perolehan tidak boleh negatif.")]
        public decimal HargaPerolehan { get; set; }

        [Required(ErrorMessage = "Deskripsi produk wajib di isi.")]
        public string Deskripsi { get; set; }

        [Required(ErrorMessage = "Stok produk wajib di isi.")]
        public int Stok { get; set; } = 0;

        // Tambahkan Foreign Key Gudang & Rak
        [Required(ErrorMessage = "Gudang wajib di isi.")]
        public int GudangId { get; set; }
        public string? NamaGudang { get; set; }

        [Required(ErrorMessage = "Rak wajib di isi.")]
        public int? RakId { get; set; }
        public string? NamaRak { get; set; }
        public int? UomId { get; set; }
        public string? NamaUom { get; set; }
    }

    public class PagedResultProduk<T>
    {
        public List<T> Items { get; set; } = new List<T>();
        public int TotalItems { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalItems / PageSize);
    }
}