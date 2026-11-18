using Microsoft.AspNetCore.Mvc;
using InventoryMultiGudang.Repository;
using Microsoft.AspNetCore.Authorization;

namespace InventoryMultiGudang.Controllers
{
    //[Authorize(Roles = "Super Admin, Manager, Warehouse Manager")]
    [Authorize(Policy = "view logs")]
    public class ActivityLogsController : Controller
    {
        private readonly IActivityLogRepository _activityLogRepository;

        public ActivityLogsController(IActivityLogRepository activityLogRepository)
        {
            _activityLogRepository = activityLogRepository;
        }

        /// <summary>
        /// Menampilkan halaman utama Activity Logs.
        /// </summary>
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Mendapatkan data log secara asynchronous via AJAX (mendukung search & pagination tanpa reload).
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetData(string search = "", int page = 1, int pageSize = 10)
        {
            try
            {
                var result = await _activityLogRepository.GetPagedLogsAsync(search, page, pageSize);

                return Json(new
                {
                    success = true,
                    message = "Data berhasil dimuat.",
                    data = result
                });
            }
            catch (Exception ex)
            {
                // Return format JSON jika terjadi error sistem/server
                return StatusCode(500, new
                {
                    success = false,
                    message = "Terjadi kesalahan internal pada server: " + ex.Message
                });
            }
        }

        /// <summary>
        /// Mengambil detail log spesifik berdasarkan ID untuk aksi lihat/detail.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetDetail(long id)
        {
            try
            {
                var log = await _activityLogRepository.GetByIdAsync(id);
                if (log == null)
                {
                    return NotFound(new
                    {
                        success = false,
                        message = "Aktivitas log tidak ditemukan."
                    });
                }

                return Json(new
                {
                    success = true,
                    message = "Detail log ditemukan.",
                    data = log
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    message = "Gagal memuat detail log: " + ex.Message
                });
            }
        }
    }
}