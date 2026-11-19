using InventoryMultiGudang.Repository;
using InventoryMultiGudang.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryMultiGudang.Controllers
{
    //[Authorize(Roles = "Super Admin, Manager, Warehouse Manager")]
    [Authorize(Policy = "manage uom")]
    public class MasterUomController : Controller
    {
        private readonly IMasterUomRepository _uomRepository;
        private readonly ILogger<MasterUomController> _logger;

        public MasterUomController(IMasterUomRepository uomRepository, ILogger<MasterUomController> logger)
        {
            _uomRepository = uomRepository;
            _logger = logger;
        }

        /// <summary>
        /// Menampilkan halaman utama UOM.
        /// </summary>
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Mengambil data tabel dan pagination via AJAX tanpa reload halaman.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetData(string search = "", int page = 1, int pageSize = 10)
        {
            try
            {
                var (data, totalCount) = await _uomRepository.GetPagedAsync(search, page, pageSize);
                var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

                return Json(new
                {
                    success = true,
                    data = data,
                    totalCount = totalCount,
                    totalPages = totalPages,
                    currentPage = page,
                    pageSize = pageSize
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saat mengambil data Master UOM.");
                return StatusCode(500, new { success = false, message = "Terjadi kesalahan pada server." });
            }
        }

        /// <summary>
        /// Mengambil data single UOM untuk keperluan Edit (Return JSON).
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var uom = await _uomRepository.GetByIdAsync(id);
                if (uom == null)
                {
                    return NotFound(new { success = false, message = "Data UOM tidak ditemukan." });
                }

                var viewModel = new UomViewModel
                {
                    Id = uom.Id,
                    Code = uom.Code,
                    Name = uom.Name,
                    Description = uom.Description,
                    IsActive = uom.IsActive
                };

                return Json(new { success = true, data = viewModel });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saat mengambil data UOM berdasarkan ID: {Id}", id);
                return StatusCode(500, new { success = false, message = "Terjadi kesalahan pada server." });
            }
        }

        /// <summary>
        /// Menyimpan data baru (Create) atau memperbarui data (Update) menggunakan Model Binder.
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save(UomViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                    return BadRequest(new { success = false, message = "Validasi gagal.", errors });
                }

                // Cek duplikasi Code
                bool codeExists = await _uomRepository.IsCodeExistsAsync(model.Code, model.Id);
                if (codeExists)
                {
                    return BadRequest(new { success = false, message = $"Kode UOM '{model.Code}' sudah terdaftar." });
                }

                if (model.Id == 0)
                {
                    // Proses Insert
                    var uom = new Models.UnitOfMeasure
                    {
                        Code = model.Code.ToUpper(),
                        Name = model.Name,
                        Description = model.Description,
                        IsActive = model.IsActive
                    };

                    var result = await _uomRepository.AddAsync(uom);
                    if (result)
                    {
                        return Json(new { success = true, message = "Data UOM berhasil ditambahkan." });
                    }
                    return StatusCode(500, new { success = false, message = "Gagal menyimpan data UOM." });
                }
                else
                {
                    // Proses Update
                    var existingUom = await _uomRepository.GetByIdAsync(model.Id);
                    if (existingUom == null)
                    {
                        return NotFound(new { success = false, message = "Data UOM tidak ditemukan." });
                    }

                    existingUom.Code = model.Code.ToUpper();
                    existingUom.Name = model.Name;
                    existingUom.Description = model.Description;
                    existingUom.IsActive = model.IsActive;

                    var result = await _uomRepository.UpdateAsync(existingUom);
                    if (result)
                    {
                        return Json(new { success = true, message = "Data UOM berhasil diperbarui." });
                    }
                    return StatusCode(500, new { success = false, message = "Gagal memperbarui data UOM." });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saat menyimpan data UOM.");
                return StatusCode(500, new { success = false, message = "Terjadi kesalahan internal pada server." });
            }
        }

        /// <summary>
        /// Menghapus data UOM berdasarkan ID.
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var uom = await _uomRepository.GetByIdAsync(id);
                if (uom == null)
                {
                    return NotFound(new { success = false, message = "Data UOM tidak ditemukan." });
                }

                var result = await _uomRepository.DeleteAsync(id);
                if (result)
                {
                    return Json(new { success = true, message = "Data UOM berhasil dihapus." });
                }

                return StatusCode(500, new { success = false, message = "Gagal menghapus data UOM." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saat menghapus data UOM dengan ID: {Id}", id);
                return StatusCode(500, new { success = false, message = "Terjadi kesalahan pada server." });
            }
        }
    }
}