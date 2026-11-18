using System.ComponentModel.DataAnnotations;

namespace InventoryMultiGudang.ViewModels
{
    public class KaryawanViewModel
    {
        public int Id { get; set; }

        public string? Nik { get; set; }

        [Required(ErrorMessage = "Nama Lengkap wajib diisi.")]
        [StringLength(100, ErrorMessage = "Nama Lengkap maksimal 100 karakter.")]
        public string NamaLengkap { get; set; } = string.Empty;

        [Required(ErrorMessage = "Alamat Email wajib diisi.")]
        [EmailAddress(ErrorMessage = "Format Alamat Email tidak valid.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Nomor Telepon / WhatsApp wajib diisi.")]
        [Phone(ErrorMessage = "Format Nomor Telepon tidak valid.")]
        public string NoTelepon { get; set; } = string.Empty;

        [Required(ErrorMessage = "Departemen Penempatan wajib dipilih.")]
        public int DepartmentId { get; set; }

        public string? DepartmentName { get; set; }

        [Required(ErrorMessage = "Status Keaktifan wajib dipilih.")]
        public string Status { get; set; } = "aktif";

        public string? Jabatan { get; set; } // diambil dari asp.net core identity role user, misalnya "Admin", "Manager", "Staff", dll.
        public string? UrlFoto { get; set; }  // simpan path foto karyawan, misalnya "images/karyawan/123.jpg"              
        public IFormFile? FotoKaryawan { get; set; } // properti untuk menyimpan file foto karyawan yang diunggah melalui form      
    }

    public class KaryawanFilterViewModel
    {
        public string? SearchTerm { get; set; }
        public int? DepartmentId { get; set; }
        public string? Status { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 5;
    }

    public class PagedResultKaryawan<T>
    {
        public List<T> Items { get; set; } = new();
        public int TotalItems { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalItems / PageSize);
    }
}