using InventoryMultiGudang.Models;
using InventoryMultiGudang.Repository;
using InventoryMultiGudang.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryMultiGudang.Controllers
{
    [Authorize(Policy = "manage loans")]
    public class PeminjamanAsetController : Controller
    {
        private readonly IPeminjamanAsetRepository _repository;

        public PeminjamanAsetController(IPeminjamanAsetRepository repository)
        {
            _repository = repository;
        }

        // Menampilkan halaman Index
        public IActionResult Index()
        {
            return View();
        }

        // Endpoint AJAX untuk Pagination, Search, dan Filter
        [HttpGet]
        public async Task<IActionResult> GetPaged(string search, StatusPeminjaman? status, int pageIndex = 1, int pageSize = 5, int? gudangId = null)
        {
            var (items, totalCount) = await _repository.GetPagedAsync(search, status, pageIndex, pageSize, gudangId);

            return Json(new
            {
                success = true,
                data = items,
                totalItems = totalCount,
                pageIndex = pageIndex,
                pageSize = pageSize
            });
        }

        // Halaman Create Form
        public IActionResult Create()
        {
            var model = new PeminjamanAsetFormViewModel();
            return View(model);
        }

        // Proses Simpan Data Create via AJAX
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PeminjamanAsetFormViewModel model)
        {
            // --- ABAIKAN VALIDASI PROPERTI PRINT & HEADER YANG TIDAK DIINPUT DARI FORM CREATE ---
            ModelState.Remove(nameof(model.KodePeminjaman));
            ModelState.Remove(nameof(model.AdminPenyerah));
            ModelState.Remove(nameof(model.PerusahaanNama));
            ModelState.Remove(nameof(model.PerusahaanSlogan));
            ModelState.Remove(nameof(model.PerusahaanAlamat));
            ModelState.Remove(nameof(model.PerusahaanTelp));
            ModelState.Remove(nameof(model.PerusahaanEmail));
            ModelState.Remove(nameof(model.KaryawanNik));
            ModelState.Remove(nameof(model.KaryawanNama));
            ModelState.Remove(nameof(model.KaryawanDivisi));
            ModelState.Remove(nameof(model.KaryawanEmail));
            ModelState.Remove(nameof(model.KaryawanTelp));

            // Bersihkan juga validasi detail untuk properti output
            if (model.Details != null)
            {
                for (int i = 0; i < model.Details.Count; i++)
                {
                    ModelState.Remove($"Details[{i}].Sku");
                    ModelState.Remove($"Details[{i}].Merek");
                    ModelState.Remove($"Details[{i}].LokasiAwal");
                    ModelState.Remove($"Details[{i}].KondisiKembali");
                }
            }

            if (!ModelState.IsValid)
            {
                // Debugging opsional untuk melihat sisa error jika masih ada
                var errorList = ModelState.Where(x => x.Value.Errors.Count > 0)
                    .Select(kvp => new { Field = kvp.Key, Errors = kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray() })
                    .ToList();

                return Json(new { success = false, message = "Validasi gagal, mohon periksa kembali form.", errors = errorList });
            }

            var result = await _repository.InsertAsync(model);
            if (result)
            {
                return Json(new { success = true, message = "Berhasil disimpan !" });
            }

            return Json(new { success = false, message = "Gagal menyimpan data peminjaman." });
        }

        // Proses Pengembalian Aset
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProsesPengembalian(int id, DateTime tanggalPengembalian, List<PeminjamanAsetDetailViewModel> updatedDetails)
        {
            var result = await _repository.ProcessReturnAsync(id, tanggalPengembalian, updatedDetails);
            if (result)
            {
                return Json(new { success = true, message = "Berhasil dikembalikan !" });
            }
            return Json(new { success = false, message = "Gagal memproses pengembalian." });
        }

        // Hapus Data Peminjaman
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _repository.DeleteAsync(id);
            if (result)
            {
                return Json(new { success = true, message = "Berhasil dihapus !" });
            }
            return Json(new { success = false, message = "Gagal menghapus data." });
        }

        [HttpGet]
        public async Task<IActionResult> GetGudangOptions(string q)
        {
            var listGudang = await _repository.GetGudangListAsync(q);

            // Format JSON disesuaikan agar kompatibel dengan plugin Select2
            var results = listGudang.Select(g => new
            {
                id = g.Id,
                text = g.NamaGudang
            });

            return Json(new { results = results });
        }

        [HttpGet]
        public async Task<IActionResult> GetAvailableAssets(string? search, string? type)
        {
            var data = await _repository.GetAvailableAssetsAsync(search, type);
            return Json(new { success = true, data = data });
        }

        [HttpGet]
        public async Task<IActionResult> GetAssetBySerial(string serialNumber)
        {
            if (string.IsNullOrWhiteSpace(serialNumber))
                return Json(new { success = false, message = "Serial number kosong." });

            var asset = await _repository.GetAssetBySerialAsync(serialNumber);

            if (asset == null)
            {
                return Json(new { success = false, message = "Aset dengan Serial Number tersebut tidak ditemukan atau sudah dipinjam!" });
            }

            return Json(new { success = true, data = asset });
        }

        [HttpGet]
        public async Task<IActionResult> GetPenerimaOptions(string? search)
        {
            var options = await _repository.GetPenerimaOptionsAsync(search);
            return Json(options);
        }

        public async Task<IActionResult> Details(int id)
        {
            var userLogin = User.Identity?.Name ?? "Administrator";
            var data = await _repository.GetByIdAsync(id, userLogin);
            if (data == null)
            {
                TempData["ErrorMessage"] = "Data pinjaman aset tidak ditemukan!";
                return RedirectToAction(nameof(Index));
            }
            return View(data);
        }

        [HttpGet]        
        public async Task<IActionResult> Print(int id)
        {
            // 1. Ambil username atau identitas user yang sedang aktif login
            string? currentUser = User.Identity?.Name;
            // 2. Konversi username login menjadi Nama Lengkap Karyawan menggunakan helper repository
            string penyerah = await _repository.GetNamaPenyerahAsync(currentUser);
            // 3. Panggil GetByIdAsync dengan menyertakan nama penyerah yang sudah valid
            var data = await _repository.GetByIdAsync(id, penyerah);
            if (data == null) return NotFound();
            return View(data);
        }
    }
}