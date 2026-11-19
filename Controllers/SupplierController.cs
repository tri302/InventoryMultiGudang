using InventoryMultiGudang.Models;
using InventoryMultiGudang.Repository;
using InventoryMultiGudang.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryMultiGudang.Controllers
{
    //[Authorize(Roles = "Super Admin, Manager, Warehouse Manager")]
    [Authorize(Policy = "manage suppliers")]
    public class SupplierController : Controller
    {
        private readonly ISupplierRepository _supplierRepo;

        public SupplierController(ISupplierRepository supplierRepo)
        {
            _supplierRepo = supplierRepo;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetData(string search, bool? status, int page = 1, int pageSize = 10)
        {
            var data = await _supplierRepo.GetPagedAsync(search, status, page, pageSize);
            return Json(data);
        }

        [HttpGet]
        public async Task<IActionResult> GetById(int id)
        {
            var item = await _supplierRepo.GetByIdAsync(id);
            if (item == null) return Json(new { success = false, message = "Data tidak ditemukan!" });

            var viewModel = new SupplierViewModel
            {
                Id = item.Id,
                KodeSupplier = item.KodeSupplier,
                NamaSupplier = item.NamaSupplier,
                Email = item.Email,
                Telepon = item.Telepon,
                NamaPic = item.NamaPic,
                WhatsappPic = item.WhatsappPic,
                Alamat = item.Alamat,
                Catatan = item.Catatan,
                IsAktif = item.IsAktif
            };

            return Json(new { success = true, data = viewModel });
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] SupplierViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                return Json(new { success = false, message = string.Join("<br>", errors) });
            }

            if (string.IsNullOrWhiteSpace(model.KodeSupplier))
            {
                model.KodeSupplier = await _supplierRepo.GenerateKodeSupplierAsync();
            }

            var entity = new Supplier
            {
                KodeSupplier = model.KodeSupplier,
                NamaSupplier = model.NamaSupplier,
                Email = model.Email,
                Telepon = model.Telepon,
                NamaPic = model.NamaPic,
                WhatsappPic = model.WhatsappPic,
                Alamat = model.Alamat,
                Catatan = model.Catatan,
                IsAktif = model.IsAktif
            };

            await _supplierRepo.AddAsync(entity);
            return Json(new { success = true, message = "Berhasil disimpan !" });
        }

        [HttpPost]
        public async Task<IActionResult> Edit([FromBody] SupplierViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                return Json(new { success = false, message = string.Join("<br>", errors) });
            }

            var entity = await _supplierRepo.GetByIdAsync(model.Id);
            if (entity == null) return Json(new { success = false, message = "Data tidak ditemukan!" });

            entity.KodeSupplier = string.IsNullOrWhiteSpace(model.KodeSupplier) ? entity.KodeSupplier : model.KodeSupplier;
            entity.NamaSupplier = model.NamaSupplier;
            entity.Email = model.Email;
            entity.Telepon = model.Telepon;
            entity.NamaPic = model.NamaPic;
            entity.WhatsappPic = model.WhatsappPic;
            entity.Alamat = model.Alamat;
            entity.Catatan = model.Catatan;
            entity.IsAktif = model.IsAktif;

            await _supplierRepo.UpdateAsync(entity);
            return Json(new { success = true, message = "Berhasil diperbarui !" });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            await _supplierRepo.DeleteAsync(id);
            return Json(new { success = true, message = "Berhasil dihapus !" });
        }
    }
}
