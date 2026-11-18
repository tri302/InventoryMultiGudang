using InventoryMultiGudang.Models;
using InventoryMultiGudang.Repository;
using Microsoft.AspNetCore.Identity;
using InventoryMultiGudang.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace InventoryMultiGudang.Controllers
{
    //[Authorize(Roles = "Super Admin, Manager, User, Warehouse Manager,Warehouse Staff, Admin")]
    [Authorize(Policy = "manage transfer orders")]
    public class TransferOrderController : Controller
    {
        private readonly ITransferOrderRepository _repository;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public TransferOrderController(ITransferOrderRepository repository, ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _repository = repository;
            _context = context;
            _userManager = userManager;
        }

        // GET: TransferOrder
        public IActionResult Index()
        {
            return View();
        }

        // GET: Ajax Load Data Table List dengan Filter & Pagination
        [HttpGet]
        public async Task<IActionResult> GetListAjax(TransferOrderFilterViewModel filter)
        {
            var pagedData = await _repository.GetPagedListAsync(filter);
            return Json(new { success = true, data = pagedData });
        }

        // GET: Detail Document Page
        public async Task<IActionResult> DetailTransferOrder(int id)
        {
            var item = await _repository.GetByIdAsync(id);
            if (item == null) return NotFound();

            return View(item);
        }

        // POST: Process Create Transfer Order
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromBody] TransferOrderViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();
                return Json(new { success = false, message = "Validasi gagal!", errors });
            }

            // === AMBIL USER ASP.NET CORE IDENTITY YANG SEDANG LOGIN ===
            string pembuat = "System"; // Fallback default
            if (User.Identity != null && User.Identity.IsAuthenticated)
            {
                var currentUser = await _userManager.GetUserAsync(User);
                if (currentUser != null)
                {
                    // Anda bisa menggunakan UserName, Email, atau properti lain (misal: Nama Lengkap jika ada)
                    pembuat = currentUser.UserName ?? currentUser.Email ?? "System";
                }
            }
            // Masukkan nama pembuat ke dalam view model atau lempar langsung ke repository
            model.DibuatOleh = pembuat;
            bool result = await _repository.CreateAsync(model);
            if (result)
            {
                return Json(new { success = true, message = "Berhasil disimpan !" });
            }
            return Json(new { success = false, message = "Gagal menyimpan data mutasi! Stok mungkin tidak mencukupi." });
        }

        // POST: Process Delete
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            bool result = await _repository.DeleteAsync(id);
            if (result)
            {
                return Json(new { success = true, message = "Berhasil dihapus !" });
            }

            return Json(new { success = false, message = "Gagal menghapus data !" });
        }

        // POST: Terima Barang (Receive)
        [HttpPost]
        public async Task<IActionResult> ReceiveBarang(int id, [FromBody] Dictionary<int, int> receivedQtyMap)
        {
            if (receivedQtyMap == null || !receivedQtyMap.Any())
            {
                return Json(new { success = false, message = "Data kuantitas penerimaan tidak boleh kosong!" });
            }

            var result = await _repository.ReceiveTransferAsync(id, receivedQtyMap);

            if (result)
            {
                return Json(new { success = true, message = "Barang berhasil diterima dan stok gudang tujuan telah diperbarui!" });
            }

            return Json(new { success = false, message = "Gagal memperbarui penerimaan barang!" });
        }

        // GET: Live Search Select2 Gudang (Max 20 data)
        [HttpGet]
        public async Task<IActionResult> GetGudangOptions(string? q)
        {
            var list = await _repository.GetGudangPagedAsync(q, 20);
            var results = list.Select(g => new { id = g.Id, text = $"{g.NamaGudang} ({g.KodeGudang})" });
            return Json(new { results });
        }
        // GET: Live Search Select2 Produk (Hanya produk dari Gudang Asal yang bersangkutan)
        [HttpGet]
        public async Task<IActionResult> GetProdukOptions(string? q, int? gudangId)
        {
            var list = await _repository.GetProdukPagedAsync(q, 20, gudangId);
            var results = list.Select(p => new
            {
                id = p.Id,
                text = $"{p.NamaProduk} - [{p.Sku}] (Stok: {p.Stok})"
            });
            return Json(new { results });
        }
        // GET: Print Document View
        public async Task<IActionResult> Print(int id)
        {
            // 1. Ambil username atau identitas user yang sedang aktif login
            string? currentUser = User.Identity?.Name;
            // 2. Konversi username login menjadi Nama Lengkap Karyawan menggunakan helper repository
            string penyerah = await _repository.GetNamaPenyerahAsync(currentUser);
            // 3. Panggil GetByIdAsync dengan menyertakan nama penyerah yang sudah valid
            var item = await _repository.GetByIdAsync(id,penyerah);
            
            if (item == null) return NotFound();

            // Ambil pengaturan sistem/perusahaan dari database (tambilkan data pertama)
            var setting = await _context.SystemSettings.FirstOrDefaultAsync();
            // Masukkan data ke ViewBag (dengan fallback teks default jika tabel kosong)
            ViewBag.SiteName = setting?.SiteName ?? "PT. CONTOH JAYA UTAMA";
            ViewBag.SiteDescription = setting?.SiteDescription ?? "INVENTORY MULTI-GUDANG MODERN";
            ViewBag.CompanyAddress = setting?.CompanyAddress ?? "Komp. Perkantoran Indah Blok B-12, Jakarta Selatan, Indonesia";
            ViewBag.CompanyPhone = setting?.CompanyPhone ?? "(021) 555-9830";
            ViewBag.CompanyEmail = setting?.CompanyEmail ?? "support@contohjayautama.com";

            return View("Print", item);
        }
    }
}