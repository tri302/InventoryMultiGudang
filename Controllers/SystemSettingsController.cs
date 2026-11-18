using Microsoft.AspNetCore.Mvc;
using InventoryMultiGudang.Models;
using InventoryMultiGudang.Repository;
using Microsoft.AspNetCore.Authorization;

namespace InventoryMultiGudang.Controllers
{
    //[Authorize(Roles = "Super Admin")]
    [Authorize(Policy = "manage settings")]
    public class SystemSettingsController : Controller
    {
        private readonly ISystemSettingRepository _settingRepo;

        public SystemSettingsController(ISystemSettingRepository settingRepo)
        {
            _settingRepo = settingRepo;
        }

        public IActionResult Index() => View();

        [HttpGet]
        public async Task<IActionResult> GetAllSettings()
        {
            var data = await _settingRepo.GetAllAsync();
            return Json(new { success = true, data });
        }

        [HttpGet]
        public async Task<IActionResult> GetById(int id)
        {
            var item = await _settingRepo.GetByIdAsync(id);
            if (item == null) return Json(new { success = false, message = "Data tidak ditemukan." });
            return Json(new { success = true, data = item });
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] SystemSetting model)
        {
            if (!ModelState.IsValid)
                return Json(new { success = false, message = "Validasi data gagal." });

            await _settingRepo.AddAsync(model);
            return Json(new { success = true, message = "Data berhasil ditambahkan!" });
        }

        [HttpPost]
        public async Task<IActionResult> Update([FromBody] SystemSetting model)
        {
            if (!ModelState.IsValid)
                return Json(new { success = false, message = "Validasi data gagal." });

            var existing = await _settingRepo.GetByIdAsync(model.Id);
            if (existing == null) return Json(new { success = false, message = "Data tidak ditemukan." });

            existing.SiteName = model.SiteName;
            existing.SiteDescription = model.SiteDescription;
            existing.CompanyAddress = model.CompanyAddress;
            existing.CompanyPhone = model.CompanyPhone;
            existing.CompanyEmail = model.CompanyEmail;

            await _settingRepo.UpdateAsync(existing);
            return Json(new { success = true, message = "Data berhasil diperbarui!" });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var existing = await _settingRepo.GetByIdAsync(id);
            if (existing == null) return Json(new { success = false, message = "Data tidak ditemukan." });

            await _settingRepo.DeleteAsync(id);
            return Json(new { success = true, message = "Data berhasil dihapus!" });
        }
    }
}