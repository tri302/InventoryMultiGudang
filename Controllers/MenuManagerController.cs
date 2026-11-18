using Microsoft.AspNetCore.Mvc;
using InventoryMultiGudang.Models;
using InventoryMultiGudang.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace InventoryMultiGudang.Controllers
{
    //[Authorize(Roles = "Super Admin")]
    [Authorize(Policy = "manage settings")]
    public class MenuManagerController : Controller
    {
        private readonly IMenuManagerRepository<MenuModel> _repo;
        private readonly ApplicationDbContext _context;

        public string message { get; private set; }

        public MenuManagerController(IMenuManagerRepository<MenuModel> repo, ApplicationDbContext context)
        {
            _repo = repo;
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Mengambil seluruh data menu item via AJAX secara async.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAllMenus()
        {
            try
            {
                var menus = await _repo.GetAllAsync();
                return Json(new { success = true, data = menus, message = "Data berhasil dimuat." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Terjadi kesalahan server: " + ex.Message });
            }
        }

        /// <summary>
        /// Menyimpan data menu baru menggunakan Repository & Unit of Work.
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateMenu([FromBody] MenuViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                return BadRequest(new { success = false, message = "Validasi gagal.", errors });
            }

            try
            {
                var menu = new MenuModel
                {
                    MenuTitle = model.MenuTitle,
                    Url = model.Url,
                    GroupName = model.GroupName,
                    ParentMenu = model.ParentMenu,
                    SortOrder = model.SortOrder,
                    IconClass = model.IconClass,
                    RequireRoles = model.SelectedRoles != null ? string.Join(",", model.SelectedRoles) : null,
                    RequirePermissions = model.SelectedPermissions != null ? string.Join(",", model.SelectedPermissions) : null
                };

                await _repo.AddAsync(menu);
                await _repo.CompleteAsync();

                return Json(new { success = true, message = "Menu item berhasil ditambahkan!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Gagal menyimpan data: " + ex.Message });
            }
        }

        /// <summary>
        /// Menghapus menu berdasarkan ID menggunakan Repository pattern.
        /// </summary>
        [HttpDelete]
        public async Task<IActionResult> DeleteMenu(int id)
        {
            try
            {
                var menu = await _repo.GetByIdAsync(id);
                if (menu == null)
                {
                    return NotFound(new { success = false, message = "Menu item tidak ditemukan." });
                }

                await _repo.DeleteAsync(menu);
                await _repo.CompleteAsync();

                return Json(new { success = true, message = "Menu item berhasil dihapus." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Gagal menghapus menu: " + ex.Message });
            }
        }

        /// <summary>
        /// Mengubah nama group menu (Rename Group) secara asinkron.
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RenameGroup([FromBody] MenuGroupViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { success = false, message = "Validasi gagal: Nama group tidak boleh kosong." });
            }

            try
            {
                // Ambil semua menu berdasarkan OldGroupName lalu update GroupName-nya
                var menus = await _repo.GetMenusByGroupAsync(model.OldGroupName);
                foreach (var menu in menus)
                {
                    menu.GroupName = model.NewGroupName;
                    _repo.Update(menu);
                }
                await _repo.CompleteAsync();

                return Json(new { success = true, message = $"Group berhasil diubah menjadi '{model.NewGroupName}'." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Gagal mengubah nama group: " + ex.Message });
            }
        }

        /// <summary>
        /// Menghapus group menu dan seluruh menu item di dalamnya.
        /// </summary>
        [HttpDelete]
        public async Task<IActionResult> DeleteGroup(string groupName)
        {
            try
            {
                var menus = await _repo.GetMenusByGroupAsync(groupName);
                foreach (var menu in menus)
                {
                    _repo.DeleteAsync(menu);
                }
                await _repo.CompleteAsync();

                return Json(new { success = true, message = $"Group '{groupName}' beserta isinya berhasil dihapus." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Gagal menghapus group: " + ex.Message });
            }
        }

        /// <summary>
        /// Mengambil daftar group unik yang ada di tabel menus secara async.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetGroups()
        {
            try
            {
                var groups = await _repo.GetDistinctGroupsAsync();
                return Json(new { success = true, data = groups });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Gagal memuat group: " + ex.Message });
            }
        }

        /// <summary>
        /// Menambahkan Group Menu baru ke database.
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateGroup([FromBody] MenuGroupViewModel model)
        {
            if (string.IsNullOrWhiteSpace(model.NewGroupName))
            {
                return BadRequest(new { success = false, message = "Nama group tidak boleh kosong." });
            }

            try
            {
                // Cek apakah group sudah ada
                var existingMenus = await _repo.GetMenusByGroupAsync(model.NewGroupName.Trim());
                if (existingMenus.Any())
                {
                    return BadRequest(new { success = false, message = "Nama group sudah ada di database." });
                }

                // Buat placeholder menu agar group terdaftar di database menus
                var placeholderMenu = new MenuModel
                {
                    MenuTitle = "[Placeholder Group]",
                    Url = "#",
                    GroupName = model.NewGroupName.Trim(),
                    SortOrder = 999,
                    IconClass = "bi-folder",
                    RequireRoles = "Admin"
                };

                await _repo.AddAsync(placeholderMenu);
                await _repo.CompleteAsync();

                return Json(new { success = true, message = $"Group '{model.NewGroupName}' berhasil ditambahkan!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Gagal menambah group: " + ex.Message });
            }
        }

        /// <summary>
        /// Mengambil daftar Roles dan Permissions dari database untuk checkbox form.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetRolesAndPermissions()
        {
            try
            {
                // Ambil data dari tabel aspnetroles & permissions menggunakan _repo atau _context
                // Contoh jika menggunakan DbContext langsung / via repository:
                var roles = await _context.Roles.Select(r => r.Name).ToListAsync();
                var permissions = await _context.Permissions.Select(p => p.Name).ToListAsync();

                return Json(new { success = true, roles = roles, permissions = permissions });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Gagal memuat roles & permissions: " + ex.Message });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetMenuById(int id)
        {
            var menu = await _repo.GetByIdAsync(id);
            if (menu == null) return NotFound(new { success = false, message = "Menu tidak ditemukan." });
            return Json(new { success = true, data = menu });
        }

        /// <summary>
        /// Menyimpan data menu baru (Insert) atau memperbarui menu yang ada (Update) berdasarkan ID.
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveMenu([FromBody] MenuViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                return BadRequest(new { success = false, message = "Validasi gagal.", errors });
            }

            try
            {
                MenuModel menu;

                if (model.Id > 0)
                {
                    // Mode Edit (Update)
                    menu = await _repo.GetByIdAsync(model.Id);
                    if (menu == null)
                    {
                        return NotFound(new { success = false, message = "Menu item tidak ditemukan." });
                    }

                    menu.MenuTitle = model.MenuTitle;
                    menu.Url = model.Url;
                    menu.GroupName = model.GroupName;
                    menu.ParentMenu = model.ParentMenu;
                    menu.SortOrder = model.SortOrder;
                    menu.IconClass = model.IconClass;
                    menu.RequireRoles = model.SelectedRoles != null ? string.Join(",", model.SelectedRoles) : null;
                    menu.RequirePermissions = model.SelectedPermissions != null ? string.Join(",", model.SelectedPermissions) : null;

                    _repo.Update(menu);
                }
                else
                {
                    // Mode Tambah Baru (Insert)
                    menu = new MenuModel
                    {
                        MenuTitle = model.MenuTitle,
                        Url = model.Url,
                        GroupName = model.GroupName,
                        ParentMenu = model.ParentMenu,
                        SortOrder = model.SortOrder,
                        IconClass = model.IconClass,
                        RequireRoles = model.SelectedRoles != null ? string.Join(",", model.SelectedRoles) : null,
                        RequirePermissions = model.SelectedPermissions != null ? string.Join(",", model.SelectedPermissions) : null
                    };

                    await _repo.AddAsync(menu);
                }

                await _repo.CompleteAsync();

                string successMsg = model.Id > 0 ? "Menu item berhasil diperbarui!" : "Menu item berhasil ditambahkan!";
                return Json(new { success = true, message = successMsg });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Gagal menyimpan data: " + ex.Message });
            }
        }
    }
}