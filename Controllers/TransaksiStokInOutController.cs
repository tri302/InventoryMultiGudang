using InventoryMultiGudang.Models;
using InventoryMultiGudang.Repository;
using InventoryMultiGudang.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InventoryMultiGudang.Controllers
{
    //[Authorize(Roles = "Super Admin, Manager, User, Warehouse Manager,Warehouse Staff, Admin")]
    [Authorize(Policy = "manage stock transactions")]
    public class TransaksiStokInOutController : Controller
    {
        private readonly ITransaksiStokRepository _repo;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public TransaksiStokInOutController(UserManager<ApplicationUser> userManager, ITransaksiStokRepository repo, ApplicationDbContext context)
        {
            _repo = repo;
            _userManager = userManager;
            _context = context;
        }

        public IActionResult Index() => View();

        [HttpGet]
        public async Task<IActionResult> GetPagedData(string? search, int? gudangId, string? tipe, int page = 1, int pageSize = 5)
        {
            var (data, total) = await _repo.GetPagedAsync(search, gudangId, tipe, page, pageSize);

            var resultData = data.Select(x => new
            {
                x.Id,
                x.NomorReferensi,
                GudangNama = x.Gudang?.NamaGudang,
                GudangKode = x.Gudang?.KodeGudang,
                x.TipeTransaksi,
                SkuCount = x.Details.Select(d => d.ProdukId).Distinct().Count(),
                TotalUnit = x.Details.Sum(d => d.Jumlah),
                x.Pembuat,
                TanggalPosting = x.TanggalPosting.ToString("dd MMM yyyy, HH.mm")
            });

            return Json(new { success = true, data = resultData, totalCount = total, page, pageSize });
        }

        // Endpoint JSON Data Detail Transaksi untuk Modal Detail
        [HttpGet]
        public async Task<IActionResult> GetDetailJson(int id)
        {
            // 1. Ambil username atau identitas user yang sedang aktif login
            string? currentUser = User.Identity?.Name;
            // 2. Konversi username login menjadi Nama Lengkap Karyawan menggunakan helper repository
            string penyerah = await _repo.GetNamaPenyerahAsync(currentUser);
            // 3. Panggil GetByIdAsync dengan menyertakan nama penyerah yang sudah valid
            var data = await _repo.GetByIdAsync(id,penyerah);            
            if (data == null)
            {
                return Json(new { success = false, message = "Data transaksi tidak ditemukan." });
            }

            var result = new
            {
                data.Id,
                data.NomorReferensi,
                data.TipeTransaksi,
                GudangNama = data.Gudang?.NamaGudang ?? "-",
                GudangKode = data.Gudang?.KodeGudang ?? "-",
                SupplierNama = data.Supplier?.NamaSupplier ?? "-",
                data.Pembuat,
                TanggalPosting = data.TanggalPosting.ToString("dd MMM yyyy, HH.mm"),
                Catatan = string.IsNullOrWhiteSpace(data.Catatan) ? "-" : data.Catatan,
                Details = data.Details.Select(d => new
                {
                    NamaProduk = d.Produk?.NamaProduk ?? "-",
                    Sku = d.Produk?.Sku ?? "-",
                    KodeLokasi = d.Rak?.KodeLokasi ?? "-",
                    NamaRak = d.Rak?.NamaRak ?? "-",
                    d.Jumlah,
                    StokAwal = data.TipeTransaksi == "Stock In" ? (d.Produk?.Stok - d.Jumlah) : (d.Produk?.Stok + d.Jumlah),
                    StokAkhir = d.Produk?.Stok
                })
            };

            return Json(new { success = true, data = result });
        }

        // Endpoint Render Halaman Print Bukti Mutasi
        [HttpGet]
        public async Task<IActionResult> Print(int id)
        {
            // 1. Ambil username atau identitas user yang sedang aktif login
            string? currentUser = User.Identity?.Name;
            // 2. Konversi username login menjadi Nama Lengkap Karyawan menggunakan helper repository
            string penyerah = await _repo.GetNamaPenyerahAsync(currentUser);
            // 3. Panggil GetByIdAsync dengan menyertakan nama penyerah yang sudah valid
            var data = await _repo.GetByIdAsync(id,penyerah);
            if (data == null)
            {
                return NotFound();
            }
            // Ambil pengaturan perusahaan dari database (Contoh menggunakan _context langsung atau via Repository)
            // Pastikan _context sudah di-inject ke dalam controller Anda
            var setting = await _context.SystemSettings.FirstOrDefaultAsync();

            // Jika belum ada data setting, berikan nilai default/fallback
            ViewBag.CompanyName = setting?.SiteName ?? "PT. CONTOH JAYA UTAMA";
            ViewBag.CompanyDesc = setting?.SiteDescription ?? "-";
            ViewBag.CompanyAddress = setting?.CompanyAddress ?? "-";
            ViewBag.CompanyPhone = setting?.CompanyPhone ?? "-";
            ViewBag.CompanyEmail = setting?.CompanyEmail ?? "-";

            return View(data);
        }

        public IActionResult StokIn()
        {
            var vm = new TransaksiStokViewModel { TipeTransaksi = "Stock In" };
            vm.Details.Add(new TransaksiStokDetailViewModel());
            return View(vm);
        }

        public IActionResult StokOut()
        {
            var vm = new TransaksiStokViewModel { TipeTransaksi = "Stock Out" };
            vm.Details.Add(new TransaksiStokDetailViewModel());
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Save([FromForm] TransaksiStokViewModel vm)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                    return Json(new { success = false, message = "Validasi gagal. Mohon periksa kembali inputan Anda.", errors });
                }

                // Ambil user yang sedang login berdasarkan HttpContext.User
                var currentUser = await _userManager.GetUserAsync(User);

                // Tentukan nilai Pembuat (fallback ke "System" jika tidak ditemukan/belum login)
                string namaPembuat = currentUser?.UserName ?? User.Identity?.Name ?? "System";

                var entity = new TransaksiStok
                {
                    Id = vm.Id,
                    GudangId = vm.GudangId,
                    SupplierId = vm.SupplierId,
                    TipeTransaksi = vm.TipeTransaksi,
                    IsPenjualan = vm.IsPenjualan,
                    NomorReferensi = string.IsNullOrWhiteSpace(vm.NomorReferensi)
                        ? $"TRX-{(vm.TipeTransaksi == "Stock In" ? "IN" : "OUT")}-{DateTime.Now:yyyyMMddHHmmss}"
                        : vm.NomorReferensi,
                    Catatan = vm.Catatan,
                    TanggalPosting = DateTime.Now,
                    Pembuat = namaPembuat, // ini dari asp.net core identyti berdasarkan user yg login.
                    Details = vm.Details.Select(d => new TransaksiStokDetail
                    {
                        ProdukId = d.ProdukId,
                        RakId = d.RakId,
                        Jumlah = d.Jumlah,
                        SerialNumbers = d.SerialNumbers
                    }).ToList()
                };

                bool isSuccess = vm.Id == 0 ? await _repo.CreateAsync(entity) : await _repo.UpdateAsync(entity);
                if (isSuccess) return Json(new { success = true, message = "Berhasil disimpan !" });
                return Json(new { success = false, message = "Gagal menyimpan data ke database!" });
            }
            catch (Exception ex)
            {
                // Kirim pesan error asli ke frontend agar muncul di Toastr
                return Json(new { success = false, message = "Error: " + ex.Message });
            }
        }


        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            bool isDeleted = await _repo.DeleteAsync(id);
            if (isDeleted) return Json(new { success = true, message = "Berhasil dihapus !" });
            return Json(new { success = false, message = "Gagal menghapus data !" });
        }

        [HttpGet]
        public async Task<IActionResult> GetDropdownGudang(string? q, int page = 1)
        {
            var results = await _repo.GetDropdownGudangAsync(q, page, 20);
            return Json(new { results, pagination = new { more = results.Count == 20 } });
        }

        [HttpGet]
        public async Task<IActionResult> GetDropdownSupplier(string? q, int page = 1)
        {
            var results = await _repo.GetDropdownSupplierAsync(q, page, 20);
            return Json(new { results, pagination = new { more = results.Count == 20 } });
        }

        [HttpGet]
        public async Task<IActionResult> GetDropdownProduk(string? q, int page = 1)
        {
            var results = await _repo.GetDropdownProdukAsync(q, page, 20);
            return Json(new { results, pagination = new { more = results.Count == 20 } });
        }

        [HttpGet]
        public async Task<IActionResult> GetDropdownRakByGudang(int? gudangId, string? q, int page = 1)
        {
            if (!gudangId.HasValue) return Json(new { results = new List<object>() });
            var results = await _repo.GetDropdownRakByGudangAsync(gudangId.Value, q, page, 20);
            return Json(new { results });
        }

        [HttpGet]
        public async Task<IActionResult> GetDropdownProdukByGudang(int? gudangId, string? q, int page = 1)
        {
            if (!gudangId.HasValue || gudangId.Value == 0)
                return Json(new { results = new List<object>() });

            var results = await _repo.GetDropdownProdukByGudangAsync(gudangId.Value, q, page, 20);
            return Json(new { results, pagination = new { more = results.Count == 20 } });
        }
        [HttpGet]
        public async Task<IActionResult> GetProdukDetail(int produkId, int gudangId, string tipeTransaksi, int jumlah = 1)
        {
            var result = await _repo.GetProdukDetailForTransactionAsync(produkId, gudangId, tipeTransaksi, jumlah);
            if (result == null)
                return Json(new { success = false, message = "Produk tidak ditemukan." });

            return Json(new { success = true, data = result });
        }

    }
}