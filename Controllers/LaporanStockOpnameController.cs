using InventoryMultiGudang.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InventoryMultiGudang.Controllers
{
    //[Authorize(Roles = "Super Admin, Manager, User, Warehouse Manager,Warehouse Staff, Admin")]    
    [Authorize(Policy = "manage stock opname")]
    public class LaporanStockOpnameController : Controller
    {
        private readonly ILaporanStockOpnameRepository _repository;
        private readonly ApplicationDbContext _context;

        public LaporanStockOpnameController(ILaporanStockOpnameRepository repository, ApplicationDbContext context)
        {
            _repository = repository;
            _context = context;
        }
        
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            // 1. Load data Gudang secara dinamis dari database
            ViewBag.GudangList = await _context.Gudangs
                .Select(g => new { g.Id, g.NamaGudang })
                .ToListAsync();

            // 2. Load Status Dokumen secara dinamis dari Enum StatusOpname
            ViewBag.StatusList = Enum.GetValues(typeof(StatusOpname))
                .Cast<StatusOpname>()
                .Select(s => new { Id = (int)s, Name = s.ToString() })
                .ToList();

            return View();
        }
        
        [HttpGet]
        public async Task<IActionResult> GetFilteredData(string startDate, string endDate, int? gudangId, int? status)
        {
            DateTime? ParseDate(string dateStr)
            {
                if (string.IsNullOrWhiteSpace(dateStr)) return null;
                // Mendukung format dd-MM-yyyy (Flatpickr) atau yyyy-MM-dd
                if (DateTime.TryParseExact(dateStr, new[] { "dd-MM-yyyy", "yyyy-MM-dd" },
                    System.Globalization.CultureInfo.InvariantCulture,
                    System.Globalization.DateTimeStyles.None, out var result))
                {
                    return result;
                }
                return null;
            }

            DateTime? parsedStartDate = ParseDate(startDate);
            DateTime? parsedEndDate = ParseDate(endDate);

            var summary = await _repository.GetSummaryReportDataAsync(parsedStartDate, parsedEndDate, gudangId, status);
            var daftarSesi = await _repository.GetDaftarSesiOpnameAsync(parsedStartDate, parsedEndDate, gudangId, status);
            var rekapitulasi = await _repository.GetRekapitulasiBarangAsync(parsedStartDate, parsedEndDate, gudangId, status);

            return Json(new
            {
                summary,
                daftarSesi,
                rekapitulasi
            });
        }
        
        [HttpGet]
        public async Task<IActionResult> Report(string startDate, string endDate, int? gudangId, int? status)
        {
            // Fungsi helper parsing tanggal dd/MM/yyyy
            DateTime? ParseDate(string dateStr)
            {
                if (string.IsNullOrWhiteSpace(dateStr)) return null;
                if (DateTime.TryParseExact(dateStr, "dd/MM/yyyy",
                    System.Globalization.CultureInfo.InvariantCulture,
                    System.Globalization.DateTimeStyles.None, out var result))
                {
                    return result;
                }
                return null;
            }

            DateTime? parsedStartDate = ParseDate(startDate);
            DateTime? parsedEndDate = ParseDate(endDate);

            // Ambil data dari repository berdasarkan filter dinamis
            var dataList = await _repository.GetDaftarSesiOpnameAsync(parsedStartDate, parsedEndDate, gudangId, status);

            // Tampilkan label filter pada header cetak laporan
            ViewBag.FilterStartDate = !string.IsNullOrWhiteSpace(startDate) ? startDate : "-";
            ViewBag.FilterEndDate = !string.IsNullOrWhiteSpace(endDate) ? endDate : "-";

            var selectedGudang = gudangId.HasValue && gudangId.Value > 0
                ? await _context.Gudangs.Where(g => g.Id == gudangId.Value).Select(g => g.NamaGudang).FirstOrDefaultAsync()
                : "Semua Gudang";
            ViewBag.FilterGudang = selectedGudang ?? "Semua Gudang";

            var selectedStatus = status.HasValue && status.Value >= 0
                ? ((StatusOpname)status.Value).ToString()
                : "Semua Status";
            ViewBag.FilterStatus = selectedStatus;

            return View(dataList);
        }

    }
}
