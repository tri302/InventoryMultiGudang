using System.ComponentModel.DataAnnotations;

namespace InventoryMultiGudang.Models
{
    public class SystemSetting
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "Site Name wajib diisi.")]
        public string SiteName { get; set; }

        public string SiteDescription { get; set; }
        public string CompanyAddress { get; set; }

        [Required(ErrorMessage = "Nomor Telepon wajib diisi.")]
        public string CompanyPhone { get; set; }

        [Required(ErrorMessage = "Email wajib diisi.")]
        [EmailAddress(ErrorMessage = "Format email tidak valid.")]
        public string CompanyEmail { get; set; }
    }
}