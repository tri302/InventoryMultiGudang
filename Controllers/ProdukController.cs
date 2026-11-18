using InventoryMultiGudang.Models;
using InventoryMultiGudang.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InventoryMultiGudang.Controllers
{
    //[Authorize(Roles = "Super Admin, Manager, Warehouse Manager")]
    [Authorize(Policy = "manage products")]
    public class ProdukController : Controller
    {
        private readonly IProdukRepository _produkRepo;
        private readonly ApplicationDbContext _context;

        public ProdukController(IProdukRepository produkRepo, ApplicationDbContext context)
        {
            _produkRepo = produkRepo;
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetData(string? search, string? kategori, int page = 1, int pageSize = 10)
        {
            var result = await _produkRepo.GetPagedAsync(search, kategori, page, pageSize);
            return Json(new { success = true, data = result });
        }

        [HttpGet]
        public async Task<IActionResult> GetById(int id)
        {
            var data = await _produkRepo.GetByIdAsync(id);
            if (data == null)
                return Json(new { success = false, message = "Data produk tidak ditemukan!" });

            return Json(new { success = true, data });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProdukViewModel model)
        {
            model.Sku = await _produkRepo.GenerateNextSkuAsync(model.Kategori);

            if (await _produkRepo.IsSkuExistsAsync(model.Sku))
            {
                return Json(new { success = false, message = "Gagal men-generate SKU unik." });
            }

            // Cek apakah Gudang yang dipilih adalah Gudang Transit Virtual
            var selectedGudang = await _context.Gudangs.FindAsync(model.GudangId);
            if (selectedGudang != null && selectedGudang.NamaGudang.Contains("Gudang Transit Virtual"))
            {
                ModelState.Remove("RakId"); // Kecualikan validasi RakId
                model.RakId = null;
            }

            ModelState.Remove("Sku");
            if (!ModelState.IsValid)
            {
                var errors = ModelState
                    .Where(x => x.Value.Errors.Count > 0)
                    .ToDictionary(k => k.Key, v => v.Value.Errors.Select(e => e.ErrorMessage).ToArray());
                return Json(new { success = false, message = "Validasi gagal! Periksa kembali inputan Anda.", errors });
            }

            var success = await _produkRepo.CreateAsync(model);
            if (success)
                return Json(new { success = true, message = "Berhasil disimpan!" });

            return Json(new { success = false, message = "Gagal menyimpan data produk!" });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ProdukViewModel model)
        {
            // Cek apakah Gudang yang dipilih adalah Gudang Transit Virtual
            var selectedGudang = await _context.Gudangs.FindAsync(model.GudangId);
            bool isVirtual = selectedGudang != null && selectedGudang.NamaGudang.Contains("Gudang Transit Virtual");

            if (isVirtual)
            {
                ModelState.Remove("RakId");
                ModelState.Remove("Sku");
                model.RakId = null;
            }

            if (!ModelState.IsValid)
            {
                var errors = ModelState
                    .Where(x => x.Value.Errors.Count > 0)
                    .ToDictionary(
                        k => k.Key,
                        v => v.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                    );

                return Json(new { success = false, message = "Validasi gagal! Periksa kembali inputan Anda.", errors });
            }

            if (await _produkRepo.IsSkuExistsAsync(model.Sku, model.Id))
            {
                return Json(new
                {
                    success = false,
                    message = "SKU sudah digunakan oleh produk lain!",
                    errors = new Dictionary<string, string[]> { { "Sku", new[] { "SKU sudah digunakan oleh produk lain!" } } }
                });
            }
            var success = await _produkRepo.UpdateAsync(model);
            if (success)
                return Json(new { success = true, message = "Berhasil diperbarui!" });
            return Json(new { success = false, message = "Gagal memperbarui data produk!" });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _produkRepo.DeleteAsync(id);
            if (success)
                return Json(new { success = true, message = "Berhasil dihapus !" });

            return Json(new { success = false, message = "Gagal menghapus produk!" });
        }
        /// <summary>
        /// Endpoint AJAX Select2 untuk Live Search Dropdown Kategori
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetDropdown(string? q, int page = 1)
        {
            int pageSize = 20;
            var result = await _produkRepo.GetPagedAsync(q, null, page, pageSize);

            var items = result.Items
                .Where(k => !string.IsNullOrEmpty(k.Kategori))
                .Select(k => new
                {
                    id = k.Kategori,
                    text = k.Kategori
                })
                .Distinct();

            return Json(new
            {
                results = items,
                pagination = new { more = (page * pageSize) < result.TotalItems }
            });
        }
        [HttpGet]
        public async Task<IActionResult> GetDropdownGudang(string? q)
        {
            var results = await _produkRepo.GetDropdownAsync(q);
            return Json(new { results });
        }

        [HttpGet]
        public async Task<IActionResult> GetDropdownRakByGudang(int? gudangId, string? q)
        {
            if (!gudangId.HasValue)
            {
                return Json(new { results = new List<DropdownItemDto>() });
            }

            var results = await _produkRepo.GetDropdownByGudangAsync(gudangId.Value, q);
            return Json(new { results });
        }

        [HttpGet]
        public async Task<IActionResult> GetSerialNumbersByProduk(int produkId)
        {
            var result = await _produkRepo.GetSerialNumbersByProdukIdAsync(produkId);

            if (result == null)
            {
                return Json(new { success = false, message = "Produk tidak ditemukan." });
            }

            return Json(new
            {
                success = true,
                namaProduk = result.NamaProduk,
                sku = result.Sku,
                items = result.Items
            });
        }

        [HttpGet]
        public async Task<IActionResult> PrintQrLabel(int produkItemId)
        {
            var itemPrintDto = await _produkRepo.GetProdukItemForPrintAsync(produkItemId);

            if (itemPrintDto == null)
            {
                return NotFound("Data unit item produk tidak ditemukan.");
            }

            return View("PrintLabel", itemPrintDto);
        }

        [HttpGet]
        public async Task<IActionResult> GenerateSku(string? kategori)
        {
            var nextSku = await _produkRepo.GenerateNextSkuAsync(kategori);
            return Json(new { success = true, sku = nextSku });
        }

        [HttpGet]
        public async Task<IActionResult> GetDropdownProduk(string? q, int page = 1)
        {
            // Contoh jika Anda nanti membuat tabel Kategori (misal: _context.Kategoris)
            var query = _context.Kategoris.AsNoTracking().AsQueryable();

            if (!string.IsNullOrWhiteSpace(q))
            {
                query = query.Where(k => k.NamaKategori.Contains(q));
            }

            var items = await query
                .Skip((page - 1) * 20)
                .Take(5)
                .Select(k => new
                {
                    id = k.NamaKategori, // atau k.Id tergantung kebutuhan value
                    text = k.NamaKategori
                })
                .ToListAsync();

            return Json(new
            {
                results = items,
                pagination = new { more = items.Count == 20 }
            });
        }

        [HttpGet]
        public async Task<IActionResult> GetDropdownKategori(string? q)
        {
            var results = await _produkRepo.GetDropdownKategoriAsync(q);
            return Json(new { results });
        }

        [HttpGet]
        public async Task<IActionResult> GetDropdownUom(string? q)
        {
            var query = _context.MasterUoms.Where(u => u.IsActive).AsNoTracking().AsQueryable();
            if (!string.IsNullOrWhiteSpace(q))
            {
                query = query.Where(u => u.Name.Contains(q) || u.Code.Contains(q));
            }
            var results = await query
                .Take(2)
                .Select(u => new DropdownItemDto
                {
                    id = u.Id,
                    text = $"{u.Code} - {u.Name}"
                })
                .ToListAsync();
            return Json(new { results });
        }

    }
}
