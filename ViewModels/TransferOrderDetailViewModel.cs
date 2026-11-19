using InventoryMultiGudang.Models;
using System.ComponentModel.DataAnnotations;

namespace InventoryMultiGudang.ViewModels
{
    public class TransferOrderDetailViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Produk wajib dipilih!")]
        public int ProdukId { get; set; }

        [Required(ErrorMessage = "Jumlah wajib diisi!")]
        [Range(1, int.MaxValue, ErrorMessage = "Jumlah minimal 1!")]
        public int KuantitasPermintaan { get; set; } = 1;

        public int KuantitasDiterima { get; set; }
        public string? NamaProduk { get; set; }
        public string? Sku { get; set; }
        public string? Satuan { get; set; }
        
        public string NoReferensi { get; set; } = string.Empty; // e.g., "TO-2026-0002"        
        public StatusTransfer Status { get; set; }

        // Gudang Pengirim
        public string GudangAsalNama { get; set; } = string.Empty;
        public string GudangAsalKode { get; set; } = string.Empty;
        public string GudangAsalAlamat { get; set; } = string.Empty;

        // Gudang Penerima
        public string GudangTujuanNama { get; set; } = string.Empty;
        public string GudangTujuanKode { get; set; } = string.Empty;
        public string GudangTujuanAlamat { get; set; } = string.Empty;

        // Timeline Transaksi
        public DateTime TanggalDibuat { get; set; }
        public DateTime? TanggalDikirim { get; set; }
        public DateTime? TanggalDiterima { get; set; }
        public string DibuatOleh { get; set; } = string.Empty;

        public string Catatan { get; set; } = string.Empty;

        // List Item Barang
        public List<TransferOrderItemViewModel> Items { get; set; } = new();

    }
}
