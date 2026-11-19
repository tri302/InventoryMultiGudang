using InventoryMultiGudang.Models;
using InventoryMultiGudang.Repository;
using InventoryMultiGudang.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryMultiGudang.Controllers
{
    //[Authorize(Roles = "Super Admin, Manager, Warehouse Manager")]
    [Authorize(Policy = "manage departments")]
    public class DepartmentController : Controller
    {
        private readonly IDepartmentRepository _repository;

        public DepartmentController(IDepartmentRepository repository)
        {
            _repository = repository;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetTableData(string? search, int page = 1, int pageSize = 5)
        {
            var result = await _repository.GetPagedAsync(search, page, pageSize);
            return PartialView("_DepartmentTablePartial", result);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DepartmentCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return Json(new { success = false, message = "Validasi gagal!", errors });
            }

            string code = string.IsNullOrWhiteSpace(model.Code)
                ? await _repository.GenerateDepartmentCodeAsync()
                : model.Code.Trim();

            if (await _repository.ExistsCodeAsync(code))
            {
                return Json(new { success = false, message = $"Kode Departemen '{code}' sudah digunakan." });
            }

            var department = new Department
            {
                Code = code,
                Name = model.Name.Trim(),
                Description = model.Description?.Trim(),
                CreatedAt = DateTime.Now
            };

            await _repository.AddAsync(department);
            return Json(new { success = true, message = "Berhasil disimpan !" });
        }

        [HttpGet]
        public async Task<IActionResult> GetById(int id)
        {
            var dept = await _repository.GetByIdAsync(id);
            if (dept == null) return Json(new { success = false, message = "Data tidak ditemukan!" });

            var model = new DepartmentEditViewModel
            {
                Id = dept.Id,
                Code = dept.Code,
                Name = dept.Name,
                Description = dept.Description
            };

            return Json(new { success = true, data = model });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(DepartmentEditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return Json(new { success = false, message = "Validasi gagal!", errors });
            }

            var dept = await _repository.GetByIdAsync(model.Id);
            if (dept == null) return Json(new { success = false, message = "Data tidak ditemukan!" });

            string code = string.IsNullOrWhiteSpace(model.Code) ? dept.Code : model.Code.Trim();

            if (await _repository.ExistsCodeAsync(code, model.Id))
            {
                return Json(new { success = false, message = $"Kode Departemen '{code}' sudah digunakan." });
            }

            dept.Code = code;
            dept.Name = model.Name.Trim();
            dept.Description = model.Description?.Trim();
            dept.UpdatedAt = DateTime.Now;

            await _repository.UpdateAsync(dept);
            return Json(new { success = true, message = "Berhasil diperbarui !" });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var dept = await _repository.GetByIdAsync(id);
            if (dept == null) return Json(new { success = false, message = "Data tidak ditemukan!" });

            await _repository.DeleteAsync(id);
            return Json(new { success = true, message = "Berhasil dihapus !" });
        }
    }
}
