namespace InventoryMultiGudang.ViewModels
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class ProdukItemViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Produk wajib dipilih.")]
        [Range(1, int.MaxValue, ErrorMessage = "Produk ID tidak valid.")]
        [Display(Name = "Produk")]
        public int ProdukId { get; set; }

        [Display(Name = "Nama Produk")]
        public string? NamaProduk { get; set; }

        [Display(Name = "SKU Produk")]
        public string? SkuProduk { get; set; }

        [Required(ErrorMessage = "Gudang lokasi item wajib dipilih.")]
        [Range(1, int.MaxValue, ErrorMessage = "Gudang tidak valid.")]
        [Display(Name = "Gudang")]
        public int GudangId { get; set; }

        [Display(Name = "Nama Gudang")]
        public string? NamaGudang { get; set; }

        [Required(ErrorMessage = "Rak penyimpanan wajib dipilih.")]
        [Range(1, int.MaxValue, ErrorMessage = "Rak tidak valid.")]
        [Display(Name = "Rak")]
        public int RakId { get; set; }

        [Display(Name = "Nama Rak")]
        public string? NamaRak { get; set; }

        [Required(ErrorMessage = "Serial Number wajib diisi.")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "Serial Number minimal 5 karakter dan maksimal 100 karakter.")]
        [RegularExpression(@"^[A-Z0-9\-_]+$", ErrorMessage = "Serial Number hanya boleh berisi huruf kapital, angka, hyphen (-), dan underscore (_).")]
        [Display(Name = "Serial Number")]
        public string SerialNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Status item wajib ditentukan.")]
        [StringLength(30, ErrorMessage = "Status maksimal 30 karakter.")]
        [RegularExpression("^(In Stock|Assigned|Loaned|Repair|Lost)$", ErrorMessage = "Status item harus salah satu dari: In Stock, Assigned, Loaned, Repair, atau Lost.")]
        [Display(Name = "Status Item")]
        public string Status { get; set; } = "In Stock";

        [Display(Name = "QR Code Image (Base64/URL)")]
        public string? QrCodePath { get; set; }

        [Display(Name = "Tanggal Buat")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }

    /// <summary>
    /// ViewModel khusus untuk pembuatan/generasi Serial Number secara massal (Batch Generation)
    /// </summary>
    public class BatchGenerateProdukItemViewModel
    {
        [Required(ErrorMessage = "Produk wajib dipilih.")]
        [Range(1, int.MaxValue, ErrorMessage = "Produk ID tidak valid.")]
        public int ProdukId { get; set; }

        [Required(ErrorMessage = "Gudang wajib dipilih.")]
        [Range(1, int.MaxValue, ErrorMessage = "Gudang ID tidak valid.")]
        public int GudangId { get; set; }

        [Required(ErrorMessage = "Rak wajib dipilih.")]
        [Range(1, int.MaxValue, ErrorMessage = "Rak ID tidak valid.")]
        public int RakId { get; set; }

        [Required(ErrorMessage = "Jumlah item (quantity) wajib diisi.")]
        [Range(1, 1000, ErrorMessage = "Jumlah pembuatan item dalam satu batch minimal 1 dan maksimal 1000.")]
        [Display(Name = "Jumlah Unit/Item")]
        public int Quantity { get; set; }
    }
}
