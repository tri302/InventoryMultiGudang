using InventoryMultiGudang.Models;
using System.ComponentModel.DataAnnotations;

namespace InventoryMultiGudang.ViewModels
{
    public class PenugasanAsetFormViewModel
    {
        public int Id { get; set; }

        public string? KodePenugasan { get; set; }

        [Required(ErrorMessage = "Tipe penerima wajib dipilih.")]
        public TipePenerima TipePenerima { get; set; } = TipePenerima.Karyawan;

        [Required(ErrorMessage = "Penerima wajib dipilih.")]
        [Display(Name = "Penerima")]
        public int TargetPenerimaId { get; set; }

        [Required(ErrorMessage = "Tanggal serah terima wajib diisi.")]
        [DataType(DataType.Date)]
        public DateTime TanggalSerahTerima { get; set; } = DateTime.Today;

        [StringLength(1000, ErrorMessage = "Keterangan tidak boleh lebih dari 1000 karakter.")]
        public string? Keterangan { get; set; }

        public List<PenugasanAsetDetailViewModel> Details { get; set; } = new List<PenugasanAsetDetailViewModel>();
    }
}
