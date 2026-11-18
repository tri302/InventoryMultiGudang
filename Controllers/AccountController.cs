using InventoryMultiGudang.Helper;
using InventoryMultiGudang.Models;
using InventoryMultiGudang.Repository;
using InventoryMultiGudang.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace InventoryMultiGudang.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<AccountController> _logger;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public AccountController(IUserRepository userRepository, ILogger<AccountController> logger, SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            _userRepository = userRepository;
            _logger = logger;
            _signInManager = signInManager;
            _userManager = userManager;
        }

        /// <summary>
        /// Menampilkan halaman utama Form Login (HTTP GET).
        /// </summary>
        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Warehouses");
            }

            ViewData["ReturnUrl"] = returnUrl;
            return View(new LoginViewModel { ReturnUrl = returnUrl });
        }

        /// <summary>
        /// Memproses permintaan login pengguna secara asinkron (HTTP POST).
        /// Mengembalikan JSON Response untuk ditangkap oleh Toastr di FE.
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProcessLogin([FromBody] LoginViewModel model)
        {
            try
            {
                // 1. Validasi State Model Backend
                if (!ModelState.IsValid)
                {
                    var errors = string.Join("; ", ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage));

                    return Json(ApiResponse<string>.FailedResult($"Validasi gagal: {errors}"));
                }

                // 2. Pencarian User di Database
                var user = await _userRepository.GetUserByEmailAsync(model.Email);
                if (user == null)
                {
                    _logger.LogWarning("Percobaan login gagal. Email tidak terdaftar: {Email}", model.Email);
                    return Json(ApiResponse<string>.NotFoundResult("Alamat email tidak ditemukan dalam sistem."));
                }

                // 3. Proses Autentikasi Password & Lockout
                var result = await _userRepository.AuthenticateUserAsync(user, model.Password, model.RememberMe, lockoutOnFailure: true);

                if (result.Succeeded)
                {
                    _logger.LogInformation("Pengguna {Email} berhasil login.", model.Email);

                    // Tentukan URL Pengalihan
                    string redirectUrl = Url.Action("Index", "Dashboard") ?? "/";
                    if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
                    {
                        redirectUrl = model.ReturnUrl;
                    }

                    return Json(ApiResponse<string>.SuccessResult("Login berhasil!", redirectUrl));
                }

                if (result.IsLockedOut)
                {
                    _logger.LogWarning("Akun {Email} dikunci karena gagal beberapa kali.", model.Email);
                    return Json(ApiResponse<string>.FailedResult("Akun Anda sementara dikunci karena terlalu banyak percobaan login yang gagal. Silakan coba lagi nanti."));
                }

                if (result.IsNotAllowed)
                {
                    return Json(ApiResponse<string>.FailedResult("Akun Anda belum dikonfirmasi atau tidak diizinkan masuk."));
                }

                // Login Gagal (Password salah)
                _logger.LogWarning("Password salah untuk akun: {Email}", model.Email);
                return Json(ApiResponse<string>.FailedResult("Email atau password yang Anda masukkan salah."));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Terjadi kesalahan internal sistem saat memproses login untuk email {Email}", model.Email);
                return Json(ApiResponse<string>.ErrorResult("Terjadi kesalahan pada server. Silakan hubungi administrator sistem."));
            }
        }

        /// <summary>
        /// Memproses logout pengguna dan menghapus cookie otentikasi.
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            try
            {
                await _userRepository.SignOutAsync();
                _logger.LogInformation("User berhasil logout.");
                return RedirectToAction("Login", "Account");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Terjadi kesalahan saat logout.");
                return RedirectToAction("Login", "Account");
            }
        }

        // ================= Change Password =================
        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = string.Join("; ", ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage));

                    return Json(ApiResponse<string>.FailedResult($"Validasi gagal: {errors}"));
                }

                // 1. Dapatkan user yang sedang login
                var user = await _userManager.GetUserAsync(User);
                if (user == null)
                {
                    return Json(ApiResponse<string>.FailedResult("Sesi pengguna telah berakhir. Silakan login kembali."));
                }

                // 2. Ubah kata sandi
                var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);

                if (result.Succeeded)
                {
                    // 3. Refresh session agar user tidak logout otomatis
                    await _signInManager.RefreshSignInAsync(user);

                    _logger.LogInformation("Kata sandi berhasil diubah untuk user {Email}", user.Email);

                    // Kirim URL tujuan redirect ke frontend
                    string redirectUrl = Url.Action("Index", "Dashboard") ?? "/";
                    return Json(ApiResponse<string>.SuccessResult("Kata sandi berhasil diubah! Mengalihkan...", redirectUrl));
                }

                // 4. Jika gagal (misal password lama salah / syarat kurang), gabungkan error
                var errorMessages = string.Join("; ", result.Errors.Select(e => e.Description));
                return Json(ApiResponse<string>.FailedResult(errorMessages));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Terjadi kesalahan saat mengubah kata sandi.");
                return Json(ApiResponse<string>.ErrorResult("Terjadi kesalahan pada server."));
            }
        }

    }
}