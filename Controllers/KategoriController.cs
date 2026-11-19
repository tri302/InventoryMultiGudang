using InventoryMultiGudang.Repository;
using InventoryMultiGudang.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryMultiGudang.Controllers
{
    //[Authorize(Roles = "Super Admin, Manager, Warehouse Manager")]
    /// <summary>
    /// Controller Pengelola Modul Kategori
    /// </summary>
    [Authorize(Policy = "manage categories")]
    public class KategoriController : Controller
    {
        private readonly IKategoriRepository _kategoriRepo;

        public KategoriController(IKategoriRepository kategoriRepo)
        {
            _kategoriRepo = kategoriRepo;
        }

        /// <summary>
        /// Menampilkan Halaman Utama Kategori
        /// </summary>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Get list data Kategori secara Async via Ajax JSON
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetData(string? search, int page = 1, int pageSize = 10)
        {
            var result = await _kategoriRepo.GetPagedAsync(search, page, pageSize);
            return Json(new { success = true, data = result });
        }

        /// <summary>
        /// Get detail Kategori berdasarkan ID via Ajax JSON
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetById(int id)
        {
            var data = await _kategoriRepo.GetByIdAsync(id);
            if (data == null)
                return Json(new { success = false, message = "Data kategori tidak ditemukan!" });

            return Json(new { success = true, data });
        }

        /// <summary>
        /// Proses Tambah Data Kategori
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(KategoriViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return Json(new { success = false, message = "Validasi gagal! Mohon lengkapi data wajib.", errors });
            }

            if (await _kategoriRepo.IsNamaKategoriExistsAsync(model.NamaKategori))
            {
                return Json(new { success = false, message = "Nama Kategori sudah terdaftar!" });
            }

            var success = await _kategoriRepo.CreateAsync(model);
            if (success)
                return Json(new { success = true, message = "Berhasil disimpan !" });

            return Json(new { success = false, message = "Gagal menyimpan data kategori!" });
        }

        /// <summary>
        /// Proses Perbarui Data Kategori
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(KategoriViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return Json(new { success = false, message = "Validasi gagal! Mohon lengkapi data wajib.", errors });
            }

            if (await _kategoriRepo.IsNamaKategoriExistsAsync(model.NamaKategori, model.Id))
            {
                return Json(new { success = false, message = "Nama Kategori sudah digunakan oleh kategori lain!" });
            }

            var success = await _kategoriRepo.UpdateAsync(model);
            if (success)
                return Json(new { success = true, message = "Berhasil diperbarui !" });

            return Json(new { success = false, message = "Gagal memperbarui data kategori!" });
        }

        /// <summary>
        /// Proses Hapus Data Kategori
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _kategoriRepo.DeleteAsync(id);
            if (success)
                return Json(new { success = true, message = "Berhasil dihapus !" });

            return Json(new { success = false, message = "Gagal menghapus kategori!" });
        }
    }
}
