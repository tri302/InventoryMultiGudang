namespace InventoryMultiGudang.Models
{
    public class Karyawan
    {
        public int Id { get; set; }
        public string Nik { get; set; } = string.Empty;
        public string NamaLengkap { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string NoTelepon { get; set; } = string.Empty;
        public int DepartmentId { get; set; }
        public Department? Department { get; set; }

        public string? Jabatan { get; set; } // diambil dari asp.net core identity role user, misalnya "Admin", "Manager", "Staff", dll.
        public string? UrlFoto { get; set; }  // simpan path foto karyawan, misalnya "images/karyawan/123.jpg"              

        public string Status { get; set; } = "aktif"; // "aktif" / "non-aktif"        
                                                      // Tambahkan baris ini untuk relasi ke PeminjamanAset
    }
}
