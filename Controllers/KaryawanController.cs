using InventoryMultiGudang.Models;
using InventoryMultiGudang.Repository;
using InventoryMultiGudang.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryMultiGudang.Controllers
{
    //[Authorize(Roles = "Super Admin, Manager, Warehouse Manager")]
    [Authorize(Policy = "manage employees")]
    public class KaryawanController : Controller
    {
        private readonly IKaryawanRepository _repo;

        public KaryawanController(IKaryawanRepository repo)
        {
            _repo = repo;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.Departments = await _repo.GetDepartmentsAsync();
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetData(KaryawanFilterViewModel filter)
        {
            var pagedData = await _repo.GetPagedAsync(filter);

            // Mencegah Json Reference Loop / Circular Reference dari Entity Framework
            var itemsDto = pagedData.Items.Select(k => new
            {
                id = k.Id,
                nik = k.Nik ?? "-",
                namaLengkap = k.NamaLengkap ?? "-",
                email = k.Email ?? "-",
                noTelepon = k.NoTelepon ?? "-",
                status = k.Status ?? "aktif",
                departmentName = k.Department != null ? k.Department.Name : "-",
                urlFoto = k.UrlFoto
            }).ToList();

            return Json(new
            {
                success = true,
                data = new
                {
                    items = itemsDto,
                    totalItems = pagedData.TotalItems,
                    pageNumber = pagedData.PageNumber,
                    pageSize = pagedData.PageSize,
                    totalPages = (int)Math.Ceiling((double)pagedData.TotalItems / pagedData.PageSize)
                }
            });
        }

        [HttpGet]
        public async Task<IActionResult> GetById(int id)
        {
            var item = await _repo.GetByIdAsync(id);
            if (item == null) return Json(new { success = false, message = "Data tidak ditemukan!" });

            var vm = new
            {
                id = item.Id,
                nik = item.Nik,
                namaLengkap = item.NamaLengkap,
                email = item.Email,
                noTelepon = item.NoTelepon,
                departmentId = item.DepartmentId,
                departmentName = item.Department != null ? item.Department.Name : null,
                status = item.Status,
                jabatan = item.Jabatan,
                urlFoto = item.UrlFoto
            };

            return Json(new { success = true, data = vm });
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] KaryawanViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                return Json(new { success = false, message = string.Join("<br/>", errors) });
            }

            if (!string.IsNullOrWhiteSpace(model.Nik) && await _repo.IsNikExistsAsync(model.Nik))
            {
                return Json(new { success = false, message = "NIK sudah digunakan oleh karyawan lain!" });
            }

            // Validasi & Handle Upload Foto
            string? urlFotoPath = null;
            if (model.FotoKaryawan != null && model.FotoKaryawan.Length > 0)
            {
                // Validasi ukuran max 2MB
                if (model.FotoKaryawan.Length > 2 * 1024 * 1024)
                {
                    return Json(new { success = false, message = "Ukuran file foto maksimal 2MB!" });
                }

                // Validasi ekstensi (.png, .jpg, .jpeg)
                var extension = Path.GetExtension(model.FotoKaryawan.FileName).ToLowerInvariant();
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
                if (!allowedExtensions.Contains(extension))
                {
                    return Json(new { success = false, message = "Format file foto hanya diizinkan .jpg, .jpeg, dan .png!" });
                }

                string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "karyawan");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                string uniqueFileName = Guid.NewGuid().ToString() + extension;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await model.FotoKaryawan.CopyToAsync(fileStream);
                }
                urlFotoPath = "images/karyawan/" + uniqueFileName;
            }

            string nik = string.IsNullOrWhiteSpace(model.Nik)
                ? await _repo.GenerateNextNikAsync()
                : model.Nik;

            var entity = new Karyawan
            {
                Nik = nik,
                NamaLengkap = model.NamaLengkap,
                Email = model.Email,
                NoTelepon = model.NoTelepon,
                DepartmentId = model.DepartmentId,
                Status = model.Status,
                Jabatan = model.Jabatan,
                UrlFoto = urlFotoPath
            };

            await _repo.AddAsync(entity);
            return Json(new { success = true, message = "Berhasil disimpan dengan NIK: " + nik });
        }

        [HttpPost]
        public async Task<IActionResult> Edit([FromForm] KaryawanViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                return Json(new { success = false, message = string.Join("<br/>", errors) });
            }

            var entity = await _repo.GetByIdAsync(model.Id);
            if (entity == null) return Json(new { success = false, message = "Data tidak ditemukan!" });

            if (!string.IsNullOrWhiteSpace(model.Nik) && await _repo.IsNikExistsAsync(model.Nik, model.Id))
            {
                return Json(new { success = false, message = "NIK sudah digunakan oleh karyawan lain!" });
            }

            // Handle Upload Foto Baru (jika ada)
            if (model.FotoKaryawan != null && model.FotoKaryawan.Length > 0)
            {
                if (model.FotoKaryawan.Length > 2 * 1024 * 1024)
                {
                    return Json(new { success = false, message = "Ukuran file foto maksimal 2MB!" });
                }

                var extension = Path.GetExtension(model.FotoKaryawan.FileName).ToLowerInvariant();
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png" };
                if (!allowedExtensions.Contains(extension))
                {
                    return Json(new { success = false, message = "Format file foto hanya diizinkan .jpg, .jpeg, dan .png!" });
                }

                string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "karyawan");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                // Hapus foto lama jika ada
                if (!string.IsNullOrEmpty(entity.UrlFoto))
                {
                    string oldPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", entity.UrlFoto);
                    if (System.IO.File.Exists(oldPath)) System.IO.File.Delete(oldPath);
                }

                string uniqueFileName = Guid.NewGuid().ToString() + extension;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await model.FotoKaryawan.CopyToAsync(fileStream);
                }
                entity.UrlFoto = "images/karyawan/" + uniqueFileName;
            }

            entity.Nik = string.IsNullOrWhiteSpace(model.Nik) ? entity.Nik : model.Nik;
            entity.NamaLengkap = model.NamaLengkap;
            entity.Email = model.Email;
            entity.NoTelepon = model.NoTelepon;
            entity.DepartmentId = model.DepartmentId;
            entity.Status = model.Status;
            entity.Jabatan = model.Jabatan;

            await _repo.UpdateAsync(entity);
            return Json(new { success = true, message = "Berhasil diperbarui !" });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null) return Json(new { success = false, message = "Data tidak ditemukan!" });

            await _repo.DeleteAsync(id);
            return Json(new { success = true, message = "Berhasil dihapus !" });
        }
        [HttpGet]
        public async Task<IActionResult> GetDepartmentsSearch(string? q)
        {
            // Selalu ambil maksimal 5 data pertama (baik saat q kosong maupun saat live search)
            var (departments, _) = await _repo.GetPagedDepartmentsAsync(q ?? "", 1, 5);

            var results = departments.Select(d => new
            {
                id = d.Id,
                text = d.Name
            });

            return Json(new
            {
                results = results,
                pagination = new { more = false } // Matikan auto-fetch halaman berikutnya
            });
        }
    }
}