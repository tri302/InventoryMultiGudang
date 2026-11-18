using System.ComponentModel.DataAnnotations;

namespace InventoryMultiGudang.ViewModels
{
    /// <summary>
    /// ViewModel untuk menangani input autentikasi halaman login.
    /// Dilengkapi dengan validasi data annotation.
    /// </summary>
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Alamat email wajib diisi.")]
        [EmailAddress(ErrorMessage = "Format email tidak valid.")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password wajib diisi.")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        public bool RememberMe { get; set; }

        public string? ReturnUrl { get; set; }
    }
}