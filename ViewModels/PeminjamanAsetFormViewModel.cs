using InventoryMultiGudang.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace InventoryMultiGudang.ViewModels
{
    public class PeminjamanAsetFormViewModel
    {
        public int Id { get; set; }
        public int KaryawanId { get; set; }

        public string? KodePeminjaman { get; set; }        

        [Required(ErrorMessage = "Tanggal pinjam wajib diisi.")]
        [Display(Name = "Tanggal Pinjam")]
        public string TanggalPinjam { get; set; } = DateTime.Now.ToString("dd/MM/yyyy");

        [Required(ErrorMessage = "Tanggal jatuh tempo wajib diisi.")]
        [Display(Name = "Tanggal Jatuh Tempo")]
        public string TanggalJatuhTempo { get; set; } = DateTime.Now.ToString("dd/MM/yyyy");

        [Display(Name = "Tanggal Pengembalian")]
        public string? TanggalPengembalian { get; set; } = DateTime.Now.ToString("dd/MM/yyyy");

        public string? TujuanPeminjaman { get; set; }

        public int? JumlahHariPinjam { get; set; }
        public int? JumlahHariTelatPinjam { get; set; }

        public StatusPeminjaman? Status { get; set; }

        // --- LIST ITEM DETAIL (DILETAKKAN DI SINI, Bukan properti satuan) ---
        [ValidateNever]
        public List<PeminjamanAsetDetailViewModel>? Details { get; set; } = new List<PeminjamanAsetDetailViewModel>();

        [ValidateNever]
        public string NamaPenerima { get; set; }
        [ValidateNever]
        public string KodePenerima { get; set; }
        [ValidateNever]
        public string EmailPenerima { get; set; }
        [ValidateNever]
        public string TeleponPenerima { get; set; }
        [ValidateNever]
        public string DivisiPenerima { get; set; }

        [ValidateNever]
        public string KaryawanNama { get; set; }
        [ValidateNever]
        public string KaryawanNik { get; set; }
        [ValidateNever]
        public string KaryawanDivisi { get; set; }
        [ValidateNever]
        public string KaryawanEmail { get; set; }
        [ValidateNever]
        public string KaryawanTelp { get; set; }

        [ValidateNever]
        public int GudangId { get; set; }

        // Properti bantu sementara untuk logika tanggal C#
        [ValidateNever]
        public DateTime _RawTanggalPinjam { get; set; }
        [ValidateNever]
        public DateTime _RawTanggalJatuhTempo { get; set; }
        [ValidateNever]
        public DateTime? _RawTanggalPengembalian { get; set; }

        // Properti Perusahaan / System Settings untuk cetak (Print)
        [ValidateNever]
        public string PerusahaanNama { get; set; }
        [ValidateNever]
        public string PerusahaanSlogan { get; set; }
        [ValidateNever]
        public string PerusahaanAlamat { get; set; }
        [ValidateNever]
        public string PerusahaanTelp { get; set; }
        [ValidateNever]
        public string PerusahaanEmail { get; set; }
        [ValidateNever]
        public string AdminPenyerah { get; set; }
    }
}