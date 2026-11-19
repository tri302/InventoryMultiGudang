using InventoryMultiGudang.Repository;
using InventoryMultiGudang.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryMultiGudang.Controllers
{
    //[Authorize(Roles = "Super Admin, Manager, Warehouse Manager")]
    [Authorize(Policy = "manage racks")]
    public class RaksController : Controller
    {
        private readonly IRakRepository _rakRepository;

        public RaksController(IRakRepository rakRepository)
        {
            _rakRepository = rakRepository;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetPagedData(int pageIndex = 1, int pageSize = 8)
        {
            var data = await _rakRepository.GetPagedAsync(pageIndex, pageSize);
            return Json(new { success = true, data = data });
        }

        [HttpGet]
        public async Task<IActionResult> GetById(int id)
        {
            var data = await _rakRepository.GetByIdAsync(id);
            if (data == null)
                return Json(new { success = false, message = "Data tidak ditemukan!" });

            return Json(new { success = true, data = data });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromBody] RakViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return Json(new { success = false, message = "Validasi gagal!", errors = errors });
            }

            if (await _rakRepository.IsKodeLokasiExistsAsync(model.KodeLokasi))
            {
                return Json(new { success = false, message = "Kode Lokasi sudah digunakan!" });
            }

            await _rakRepository.CreateAsync(model);
            return Json(new { success = true, message = "Berhasil disimpan !" });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([FromBody] RakViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return Json(new { success = false, message = "Validasi gagal!", errors = errors });
            }

            if (await _rakRepository.IsKodeLokasiExistsAsync(model.KodeLokasi, model.Id))
            {
                return Json(new { success = false, message = "Kode Lokasi sudah digunakan!" });
            }

            await _rakRepository.UpdateAsync(model);
            return Json(new { success = true, message = "Berhasil diperbarui !" });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var rak = await _rakRepository.GetByIdAsync(id);
            if (rak == null)
            {
                return Json(new { success = false, message = "Data tidak ditemukan!" });
            }

            await _rakRepository.DeleteAsync(id);
            return Json(new { success = true, message = "Berhasil dihapus !" });
        }
        [HttpGet]
        public async Task<IActionResult> GetGudangOptions(string? search, int page = 1)
        {
            var result = await _rakRepository.GetGudangOptionsAsync(search, page);
            return Json(result);
        }
    }
}