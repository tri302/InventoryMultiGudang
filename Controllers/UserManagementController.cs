using InventoryMultiGudang.Models;
using InventoryMultiGudang.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryMultiGudang.Controllers
{
    //[Authorize(Roles = "Super Admin")]
    [Authorize(Policy = "manage users")]
    public class UserManagementController : Controller
    {
        private readonly IUserRepository _userRepository;

        public UserManagementController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.Roles = await _userRepository.GetAllRolesAsync();
            return View();
        }

        // Endpoint AJAX untuk Load Data & Pagination Tabel
        [HttpGet]
        public async Task<IActionResult> GetData(string search, string role, int page = 1)
        {
            int pageSize = 5;
            var (users, totalCount) = await _userRepository.GetPagedUsersAsync(search, role, page, pageSize);

            var viewModel = new UserListViewModel
            {
                Users = users,
                CurrentPage = page,
                TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize),
                TotalUsers = totalCount
            };

            return PartialView("_UserTablePartial", viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> GetById(string id)
        {
            var user = await _userRepository.GetByIdAsync(id);
            if (user == null) return NotFound();
            return Json(user);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] UserViewModel model)
        {
            // --- DEBUG DI BACKEND ---
            // Console.WriteLine($"--- DEBUG CREATE USER ---");
            // Console.WriteLine($"FullName: {model.FullName}");
            // Console.WriteLine($"Email: {model.Email}");
            // Console.WriteLine($"Roles Count: {model.Roles?.Count ?? 0}");
            // if (model.Roles != null)
            // {
            //     foreach (var r in model.Roles)
            //     {
            //         Console.WriteLine($"Role Item: '{r}'");
            //     }
            // }
            // -------------------------

            // Validasi Password khusus Create
            if (string.IsNullOrWhiteSpace(model.Password))
            {
                return Json(new { success = false, message = "Password wajib diisi saat membuat user baru." });
            }

            if (model.Roles == null || !model.Roles.Any())
            {
                return Json(new { success = false, message = "Pilih minimal satu Security Role!" });
            }

            var (createSuccess, createErrors) = await _userRepository.CreateAsync(model);

            if (createSuccess)
            {
                return Json(new { success = true, message = "User berhasil ditambahkan!" });
            }

            return Json(new { success = false, message = string.Join("<br>", createErrors) });
        }

        [HttpPost]
        public async Task<IActionResult> Update([FromBody] UserViewModel model)
        {
            if (string.IsNullOrEmpty(model.Id))
            {
                return Json(new { success = false, message = "User ID tidak valid." });
            }

            if (model.Roles == null || !model.Roles.Any())
            {
                return Json(new { success = false, message = "Pilih minimal satu Security Role!" });
            }

            var (success, errors) = await _userRepository.UpdateAsync(model);
            if (success)
            {
                return Json(new { success = true, message = "User berhasil diperbarui!" });
            }
            return Json(new { success = false, message = string.Join("<br>", errors) });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var success = await _userRepository.DeleteAsync(id);
            if (success) return Json(new { success = true, message = "User berhasil dihapus!" });
            return Json(new { success = false, message = "Gagal menghapus user." });
        }

        [HttpGet]
        public async Task<IActionResult> GetKaryawan(string? term)
        {            
            var karyawans = await _userRepository.SearchKaryawansAsync(term, takeLimit: 5);
            var results = karyawans.Select(k => new
            {
                id = k.Id,
                text = $"{k.Nik} - {k.NamaLengkap} ({k.Jabatan ?? k.Email})"
            });
            return Json(new { results = results });
        }

    }
}