using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryMultiGudang.Models
{
    public enum TipePenerima
    {
        Karyawan = 1,
        Departemen = 2,
        Customer = 3
    }

    public enum StatusPenugasan
    {
        [Display(Name = "Pending Approval")]
        MenungguPersetujuan = 1,

        [Display(Name = "Assigned")]
        DalamPenugasan = 2,

        [Display(Name = "Returned")]
        Dikembalikan = 3,

        [Display(Name = "Rejected")]
        Ditolak = 4,

        [Display(Name = "Direct Asset Transfer")]
        TransferAsetLangsung = 5

        // MenungguPersetujuan = 1,
        // DalamPenugasan = 2,
        // Dikembalikan = 3,
        // Ditolak = 4,
        // TransferAsetLangsung = 5
    }

    public class PenugasanAset
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(50)]
        public string KodePenugasan { get; set; } = string.Empty;

        public TipePenerima TipePenerima { get; set; }

        [NotMapped]
        public string? NamaTarget { get; set; }

        public int TargetPenerimaId { get; set; } // Id Karyawan / Department / Customer

        public DateTime TanggalSerahTerima { get; set; }

        public DateTime? TanggalPengembalian { get; set; }

        [StringLength(1000)]
        public string? Keterangan { get; set; }

        public StatusPenugasan Status { get; set; } = StatusPenugasan.DalamPenugasan;

        public virtual ICollection<PenugasanAsetDetail> Details { get; set; } = new List<PenugasanAsetDetail>();

        [NotMapped]
        public string PerusahaanNama { get; set; }

        [NotMapped]
        public string PerusahaanSlogan { get; set; }

        [NotMapped]
        public string PerusahaanAlamat { get; set; }
        [NotMapped]
        public string PerusahaanTelp { get; set; }
        [NotMapped]
        public string PerusahaanEmail { get; set; }
        [NotMapped]
        public string KaryawanNama { get; set; }
        [NotMapped]
        public string KaryawanNik { get; set; }
        [NotMapped]
        public string KaryawanDivisi { get; set; }
        [NotMapped]
        public string KaryawanEmail { get; set; }
        [NotMapped]
        public string KaryawanTelp { get; set; }
        [NotMapped]
        public string AdminPenyerah { get; set; }
        [NotMapped]
        public string CustomerNama { get; set; }
        [NotMapped]
        public string CustomerKode { get; set; }
        [NotMapped]
        public string CustomerAlamat { get; set; }
        [NotMapped]
        public string DepartemenNama { get; set; }
        [NotMapped]
        public string DepartemenKode { get; set; }
        [NotMapped]
        public string DepartemenKeterangan { get; set; }
        [NotMapped]
        public int GudangId { get; set; }
        [NotMapped]
        public int ProdukId { get; set; }
        [NotMapped]
        public int Jumlah { get; set; }
        [NotMapped]
        public string NoPenugasan { get; set; }
        [NotMapped]
        public string? Pembuat { get; set; }
    }
}