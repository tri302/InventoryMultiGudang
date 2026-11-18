using InventoryMultiGudang.Models;
using InventoryMultiGudang.Repository;
using InventoryMultiGudang.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace InventoryMultiGudang.Controllers
{
    //[Authorize(Roles = "Super Admin, Manager, User, Warehouse Manager,Warehouse Staff, Admin")]
    [Authorize(Policy = "view stock cards")]
    public class KartuStok : Controller
    {
        private readonly IKartuStokRepository _repository;
        private readonly ILogger<KartuStok> _logger;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public KartuStok(
            IKartuStokRepository repository,
            ILogger<KartuStok> logger, ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _repository = repository;
            _logger = logger;
            _context = context;
            _userManager = userManager;
        }
        /// <summary>
        /// Menampilkan halaman utama laporan kartu stok.
        /// Data histori tidak langsung di-load sampai filter dipilih.
        /// </summary>
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> SearchProduk(int gudangId, string? term, CancellationToken cancellationToken)
        {
            try
            {
                // Jika repository Anda membutuhkan gudangId, sesuaikan panggilannya
                var data = await _repository.SearchProdukAsync(term ?? string.Empty, 5, gudangId);
                return Json(new { success = true, data });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Gagal mencari produk.");
                return StatusCode(500, new { success = false, message = "Gagal memuat produk." });
            }
        }
        /// <summary>
        /// Mencari produk berdasarkan gudang secara AJAX.
        /// Hanya maksimal 5 data dikembalikan.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> SearchGudang(string? term, CancellationToken cancellationToken)
        {
            try
            {
                var data = await _repository.SearchGudangAsync(term ?? string.Empty, 5);
                return Json(new { success = true, data });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Gagal mencari gudang.");
                return StatusCode(500, new { success = false, message = "Gagal memuat gudang." });
            }
        }
        /// <summary>
        /// Mengambil histori kartu stok berdasarkan gudang dan produk.
        /// Response digunakan langsung oleh AJAX untuk merender Details.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Details(int gudangId, int produkId, CancellationToken cancellationToken = default)
        {
            try
            {
                if (gudangId <= 0 || produkId <= 0)
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "Gudang dan produk wajib dipilih."
                    });
                }
                var result = await _repository.GetDetailsAsync(
                    new KartuStokFilterViewModel
                    {
                        GudangId = gudangId,
                        ProdukId = produkId
                    },
                    cancellationToken);
                if (result == null)
                {
                    return NotFound(new
                    {
                        success = false,
                        message = "Data kartu stok tidak ditemukan."
                    });
                }
                return Json(new
                {
                    success = true,
                    message = "Data kartu stok berhasil dimuat.",
                    data = result
                });
            }
            catch (OperationCanceledException)
            {
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Gagal mengambil kartu stok. GudangId={GudangId}, ProdukId={ProdukId}",
                    gudangId,
                    produkId);
                return StatusCode(
                    StatusCodes.Status500InternalServerError,
                    new
                    {
                        success = false,
                        message = "Terjadi kesalahan saat mengambil kartu stok."
                    });
            }
        }

        /// <summary>
        /// Menampilkan halaman report kartu stok.
        /// Parameter tetap berasal dari filter aktif di halaman Index.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> ReportKartuStok(int gudangId, int produkId, CancellationToken cancellationToken)
        {
            try
            {
                if (gudangId <= 0 || produkId <= 0)
                {
                    TempData["ReportError"] = "Silakan pilih Gudang dan Produk terlebih dahulu.";
                    return RedirectToAction(nameof(Index));
                }

                // Ambil data pengaturan sistem untuk ViewBag
                var settings = await _context.SystemSettings.AsNoTracking().FirstOrDefaultAsync(cancellationToken);
                ViewBag.SiteName = settings?.SiteName ?? "PT. CONTOH JAYA UTAMA";
                ViewBag.SiteDescription = settings?.SiteDescription ?? "-";
                ViewBag.CompanyAddress = settings?.CompanyAddress ?? "-";
                ViewBag.CompanyPhone = settings?.CompanyPhone ?? "-";
                ViewBag.CompanyEmail = settings?.CompanyEmail ?? "-";

                // 1. Ambil username atau identitas user yang sedang aktif login
                string? currentUser = User.Identity?.Name;
                // 2. Konversi username login menjadi Nama Lengkap Karyawan menggunakan helper repository
                string penyerah = await _repository.GetNamaPenyerahAsync(currentUser);
                // 3. Panggil GetByIdAsync dengan menyertakan nama penyerah yang sudah valid            
                var result = await _repository.GetReportKartuStokAsync(gudangId, produkId, penyerah);
                if (result == null)
                {
                    TempData["ReportError"] = "Data kartu stok tidak ditemukan.";
                    return RedirectToAction(nameof(Index));
                }
                // Timpa nilai Operator dengan user yang sedang login saat ini
                result.Operator = penyerah;

                return View(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Gagal membuka report kartu stok.");
                TempData["ReportError"] = "Report kartu stok gagal dibuka.";
                return RedirectToAction(nameof(Index));
            }
        }

        /// <summary>
        /// Mengambil 5 data gudang untuk initial dropdown.
        /// Endpoint digunakan oleh AJAX tanpa reload halaman.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetInitialGudang()
        {
            try
            {
                var data = await _repository.GetInitialGudangAsync();
                return Json(new
                {
                    success = true,
                    data
                });
            }
            catch (Exception)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Gagal mengambil data gudang."
                });
            }
        }
        [HttpGet]
        public async Task<IActionResult> GetInitialProduk()
        {
            try
            {
                var data = await _repository.GetInitialProdukAsync();
                return Json(new { success = true, data });
            }
            catch (Exception)
            {
                return StatusCode(500, new { success = false, message = "Gagal mengambil data produk." });
            }
        }
    }
}
