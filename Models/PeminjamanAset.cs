using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryMultiGudang.Models
{

    public enum StatusPeminjaman
    {
        Terlambat = 1,
        Kembali = 2,
        Dipinjam = 3,
    }

    [Table("peminjamanaset")]
    public class PeminjamanAset
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string KodePeminjaman { get; set; }            

        [Required]
        [DataType(DataType.Date)]
        public DateTime TanggalPinjam { get; set; } = DateTime.Now;

        [Required]
        [DataType(DataType.Date)]
        public DateTime TanggalJatuhTempo { get; set; }

        [DataType(DataType.Date)]
        public DateTime? TanggalPengembalian { get; set; }

        [StringLength(500)]
        public string? TujuanPeminjaman { get; set; }

        public int? JumlahHariPinjam { get; set; }
        public int? JumlahHariTelatPinjam { get; set; }

        public StatusPeminjaman? Status { get; set; } = StatusPeminjaman.Dipinjam; // Dipinjam, Kembali, Terlambat

        // Navigasi Relasi ke Detail (1 to Many)
        
        public virtual ICollection<PeminjamanAsetDetail> Details { get; set; } = new List<PeminjamanAsetDetail>();
        
        public int GudangId { get; set; }

        public int? KaryawanId { get; set; }
        [ForeignKey("KaryawanId")]
        public virtual Karyawan? Karyawan { get; set; }

        [NotMapped]
        [ValidateNever]
        public int ProdukId { get; set; }
        [NotMapped]
        [ValidateNever]
        public string? Pembuat { get; set; }
        [NotMapped]
        [ValidateNever]
        public string Peminjam { get; set; }
    }
}