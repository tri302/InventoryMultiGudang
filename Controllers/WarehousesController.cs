using InventoryMultiGudang.Repository;
using InventoryMultiGudang.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryMultiGudang.Controllers
{
    //[Authorize(Roles = "Super Admin, Manager, Warehouse Manager")]
    [Authorize(Policy = "manage warehouses")]
    public class WarehousesController : Controller
    {
        private readonly IGudangRepository _repo;

        public WarehousesController(IGudangRepository repo)
        {
            _repo = repo;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetList(int pageIndex = 1, int pageSize = 6, string? search = "", string? tipe = "")
        {
            var result = await _repo.GetPagedAsync(pageIndex, pageSize, search, tipe);
            return Json(new { success = true, data = result });
        }

        [HttpGet]
        public async Task<IActionResult> GetById(int id)
        {
            var data = await _repo.GetByIdAsync(id);
            if (data == null)
                return Json(new { success = false, message = "Data gudang tidak ditemukan." });

            return Json(new { success = true, data });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromBody] GudangViewModel model)
        {
            // Validasi ModelState umum (selain KodeGudang karena di-generate otomatis)
            if (!ModelState.IsValid)
            {
                var errors = ModelState
                    .Where(x => x.Value?.Errors.Count > 0)
                    .ToDictionary(
                        kvp => kvp.Key,
                        kvp => kvp.Value?.Errors.Select(e => e.ErrorMessage).ToArray()
                    );
                return Json(new { success = false, message = "Validasi gagal, periksa kembali inputan Anda.", errors });
            }
            await _repo.CreateAsync(model);
            return Json(new { success = true, message = "Berhasil disimpan! Kode Gudang digenerate otomatis." });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update([FromBody] GudangViewModel model)
        {
            // Cek apakah Kode Gudang sudah dipakai oleh data lain (kecuali data ini sendiri)
            if (await _repo.IsKodeExistsAsync(model.KodeGudang, model.Id))
            {
                ModelState.AddModelError("KodeGudang", "Kode Gudang sudah digunakan!");
            }

            if (!ModelState.IsValid)
            {
                var errors = ModelState
                    .Where(x => x.Value?.Errors.Count > 0)
                    .ToDictionary(
                        kvp => kvp.Key,
                        kvp => kvp.Value?.Errors.Select(e => e.ErrorMessage).ToArray()
                    );
                return Json(new { success = false, message = "Validasi gagal, periksa kembali inputan Anda.", errors });
            }

            await _repo.UpdateAsync(model);
            return Json(new { success = true, message = "Berhasil diperbarui!" });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var existing = await _repo.GetByIdAsync(id);
            if (existing == null)
                return Json(new { success = false, message = "Data gudang tidak ditemukan!" });

            await _repo.DeleteAsync(id);
            return Json(new { success = true, message = "Berhasil dihapus !" });
        }

    }
}