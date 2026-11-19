using InventoryMultiGudang.Models;
using InventoryMultiGudang.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryMultiGudang.Controllers
{
    //[Authorize(Roles = "Super Admin, Manager, User, Warehouse Manager,Warehouse Staff, Admin")]
    [Authorize(Policy = "manage reparasi aset")]
    public class ReparasiAsetsController : Controller
    {
        private readonly IReparasiAsetsRepository _repository;
        private readonly ApplicationDbContext _context;

        public ReparasiAsetsController(IReparasiAsetsRepository repository, ApplicationDbContext context)
        {
            _repository = repository;
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetReparasiData(string search = "", int page = 1, int pageSize = 10)
        {
            var (items, totalCount) = await _repository.GetReparasiListAsync(search, page, pageSize);
            return Json(new { items, totalCount, currentPage = page, totalPages = (int)Math.Ceiling(totalCount / (double)pageSize) });
        }

        [HttpPost]
        public async Task<IActionResult> SaveReparasi([FromBody] ReparasiAsset model)
        {
            // Validasi Backend (ModelState)
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                return BadRequest(new { success = false, message = string.Join(" | ", errors) });
            }

            bool result = await _repository.SaveOrUpdateReparasiAsync(model);
            return Json(new { success = result, message = result ? "Data reparasi berhasil disimpan." : "Gagal menyimpan data." });
        }

        [HttpGet]
        public async Task<IActionResult> GetReparasiById(int id)
        {
            var data = await _repository.GetReparasiByIdAsync(id);
            if (data == null) return NotFound();
            return Json(data);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteReparasi(int id)
        {
            bool result = await _repository.DeleteReparasiAsync(id);
            return Json(new { success = result, message = result ? "Data berhasil dihapus." : "Gagal menghapus data." });
        }

        // Endpoint untuk Select2 Aset (Batasi 5 data per load + Search)
        [HttpGet]
        public async Task<IActionResult> GetProdukItemOptions(string q = "", int page = 1)
        {
            int pageSize = 5; //tampilkan 5 data awal/per load
            var (items, more) = await _repository.GetProdukItemSelect2Async(q, page, pageSize);

            return Json(new
            {
                items = items,
                pagination = new
                {
                    more = more
                }
            });
        }


    }
}