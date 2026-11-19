using System.ComponentModel.DataAnnotations;

namespace InventoryMultiGudang.ViewModels
{
    public class SupplierViewModel
    {
        public int Id { get; set; }

        public string KodeSupplier { get; set; }

        [Required(ErrorMessage = "Nama Supplier wajib diisi!")]
        [StringLength(150, ErrorMessage = "Nama Supplier maksimal 150 karakter.")]
        public string NamaSupplier { get; set; }

        [Required(ErrorMessage = "Email wajib diisi!")]
        [EmailAddress(ErrorMessage = "Format Email tidak valid.")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Telepon Kantor wajib diisi!")]
        [Phone(ErrorMessage = "Format nomor telepon tidak valid.")]
        public string Telepon { get; set; }

        [Required(ErrorMessage = "Nama PIC wajib diisi!")]
        public string NamaPic { get; set; }

        [Required(ErrorMessage = "WhatsApp PIC wajib diisi!")]
        public string WhatsappPic { get; set; }

        [Required(ErrorMessage = "Alamat Perusahaan wajib diisi!")]
        public string Alamat { get; set; }

        public string Catatan { get; set; }       

        public bool IsAktif { get; set; } = true;
    }

    public class SupplierPagedResultViewModel
    {
        public List<SupplierViewModel> Items { get; set; } = new List<SupplierViewModel>();
        public int TotalItems { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalItems / PageSize);
    }
}
