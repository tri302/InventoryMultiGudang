using Microsoft.AspNetCore.Identity;

namespace InventoryMultiGudang.Models
{
    public class ApplicationUser : IdentityUser
    {
        public DateTime CreatedAt { get; set; } = DateTime.Now;        
        public string FullName { get; set; } = string.Empty;
        // Tambahkan Foreign Key ke tabel Karyawans
        public int? KaryawanId { get; set; }
        public virtual Karyawan? Karyawan { get; set; }
    }
}
