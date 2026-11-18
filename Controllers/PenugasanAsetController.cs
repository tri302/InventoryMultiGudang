using InventoryMultiGudang.Models;
using InventoryMultiGudang.Repository;
using InventoryMultiGudang.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InventoryMultiGudang.Controllers
{
    //[Authorize(Roles = "Super Admin, Manager, User, Warehouse Manager,Warehouse Staff, Admin")]
    [Authorize(Policy = "manage assignments")]
    public class PenugasanAsetController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly IPenugasanAsetRepository _repo;
        private readonly IWebHostEnvironment _env;

        public PenugasanAsetController(ApplicationDbContext db, IPenugasanAsetRepository repo, IWebHostEnvironment env)
        {
            _db = db;
            _repo = repo;
            _env = env;
        }
        public IActionResult TransferAsset()
        {
            return View();
        }

        // GET: /PenugasanAset/Index
        public IActionResult Index()
        {
            return View();
        }

        // AJAX GET: /PenugasanAset/GetPaged
        [HttpGet]
        public async Task<IActionResult> GetPaged(string? search, int? status, int page = 1, int pageSize = 5)
        {
            var data = await _repo.GetPagedAsync(search, status, page, pageSize);
            return Json(new { success = true, data });
        }

        // GET: /PenugasanAset/Create
        public async Task<IActionResult> Create()
        {
            var vm = new PenugasanAsetFormViewModel
            {
                KodePenugasan = await _repo.GenerateKodePenugasanAsync(),
                TanggalSerahTerima = DateTime.Today
            };
            return View(vm);
        }

        // POST: /PenugasanAset/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromForm] PenugasanAsetFormViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return Json(new { success = false, message = "Validasi gagal. Mohon periksa kembali inputan Anda.", errors });
            }
            // Jika KodePenugasan kosong dari form, generate ulang di server
            string kodePenugasan = string.IsNullOrWhiteSpace(vm.KodePenugasan)
                ? await _repo.GenerateKodePenugasanAsync()
                : vm.KodePenugasan;

            var domainModel = new PenugasanAset
            {
                KodePenugasan = kodePenugasan,
                TipePenerima = (TipePenerima)vm.TipePenerima,
                TargetPenerimaId = vm.TargetPenerimaId,
                TanggalSerahTerima = vm.TanggalSerahTerima,
                Keterangan = vm.Keterangan,
                Status = StatusPenugasan.MenungguPersetujuan,
                Details = vm.Details.Select(d => new PenugasanAsetDetail
                {
                    NamaBarang = d.NamaBarang,
                    SerialNumber = d.SerialNumber,
                    LokasiAwal = d.LokasiAwal,
                    Kuantitas = d.Kuantitas,
                    KondisiSerah = d.KondisiSerah
                }).ToList()
            };

            await _repo.CreateAsync(domainModel);
            return Json(new { success = true, message = "Berhasil disimpan !" });
        }

        // GET: /PenugasanAset/DetailsKaryawan/5
        public async Task<IActionResult> DetailsKaryawan(int id)
        {
            var data = await _repo.GetByIdAsync(id);
            if (data == null) return NotFound();

            // Load data relasi penerima lengkap ke ViewBag
            await PopulatePenerimaInfoToViewBag(data.TipePenerima, data.TargetPenerimaId);

            return View(data);
        }

        // GET: /PenugasanAset/DetailsDepartemen/5
        public async Task<IActionResult> DetailsDepartemen(int id)
        {
            var data = await _repo.GetByIdAsync(id);
            if (data == null) return NotFound();

            // Load data relasi penerima lengkap ke ViewBag
            await PopulatePenerimaInfoToViewBag(data.TipePenerima, data.TargetPenerimaId);

            return View(data);
        }

        // GET: /PenugasanAset/DetailsCustomer/5
        public async Task<IActionResult> DetailsCustomer(int id)
        {
            var data = await _repo.GetByIdAsync(id);
            if (data == null) return NotFound();

            // Load data relasi penerima lengkap ke ViewBag
            await PopulatePenerimaInfoToViewBag(data.TipePenerima, data.TargetPenerimaId);

            return View(data);
        }

        // POST: /PenugasanAset/Delete/5
        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var data = await _repo.GetByIdAsync(id);
            if (data == null)
            {
                return Json(new { success = false, message = "Data penugasan tidak ditemukan!" });
            }

            await _repo.DeleteAsync(id);
            return Json(new { success = true, message = "Berhasil dihapus !" });
        }
        [HttpGet]
        public async Task<IActionResult> GetPenerimaOptions(int tipe, string? search)
        {
            // Konversi integer tipe dari JS (0: Karyawan, 1: Departemen, 2: Customer) ke Enum
            var tipeEnum = (TipePenerima)tipe;

            var options = await _repo.GetPenerimaOptionsAsync(tipeEnum, search);

            // Kembalikan langsung array JSON agar Select2 tidak kebingungan saat parsing
            return Json(options);
        }
        [HttpGet]
        public async Task<IActionResult> GetAvailableAssets(string? search, string? type)
        {
            var data = await _repo.GetAvailableAssetsAsync(search, type);
            return Json(new { success = true, data = data });
        }

        [HttpGet]
        public async Task<IActionResult> GetAssetBySerial(string serialNumber)
        {
            if (string.IsNullOrWhiteSpace(serialNumber))
                return Json(new { success = false, message = "Serial number kosong." });

            var asset = await _repo.GetAssetBySerialAsync(serialNumber);

            if (asset == null)
            {
                return Json(new { success = false, message = "Aset dengan Serial Number tersebut tidak ditemukan atau sudah ditugaskan!" });
            }

            return Json(new { success = true, data = asset });
        }
        // GET: /PenugasanAset/Print/5
        [HttpGet]
        public async Task<IActionResult> PrintKaryawan(int id)
        {
            string? currentUser = User.Identity?.Name;
            var printVm = await _repo.GetPrintKaryawanViewModelAsync(id, currentUser);

            if (printVm == null)
            {
                TempData["ErrorMessage"] = "Data penugasan aset karyawan tidak ditemukan!";
                return RedirectToAction(nameof(Index));
            }

            return View(printVm);
        }

        [HttpGet]        
        public async Task<IActionResult> PrintCustomer(int id)
        {
            string? currentUser = User.Identity?.Name;
            var printVm = await _repo.GetPrintCustomerViewModelAsync(id, currentUser);

            if (printVm == null)
            {
                TempData["ErrorMessage"] = "Data penugasan aset customer tidak ditemukan!";
                return RedirectToAction(nameof(Index));
            }

            return View(printVm);
        }

        [HttpGet]
        public async Task<IActionResult> PrintDepartment(int id)
        {
            string? currentUser = User.Identity?.Name;
            var printVm = await _repo.GetPrintDepartmentViewModelAsync(id, currentUser);

            if (printVm == null)
            {
                TempData["ErrorMessage"] = "Data penugasan aset departemen tidak ditemukan!";
                return RedirectToAction(nameof(Index));
            }

            return View(printVm);
        }

        // POST: /PenugasanAset/ProsesPengembalian
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProsesPengembalian(int Id, DateTime TanggalPengembalian, List<PenugasanAsetDetailViewModel> updatedDetails)
        {
            if (Id <= 0)
            {
                return Json(new { success = false, message = "ID Penugasan tidak valid!" });
            }

            try
            {
                await _repo.ProcessPengembalianAsync(Id, TanggalPengembalian, updatedDetails);

                // Cek jika request berasal dari AJAX atau Normal Form Submit
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new { success = true, message = "Pengembalian aset berhasil diproses!" });
                }

                return RedirectToAction("Details", new { id = Id });
            }
            catch (Exception ex)
            {
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new { success = false, message = "Terjadi kesalahan: " + ex.Message });
                }
                return BadRequest(ex.Message);
            }
        }

        // Helper untuk mengisi informasi detail penerima
        private async Task PopulatePenerimaInfoToViewBag(TipePenerima tipe, int targetId)
        {
            switch (tipe)
            {
                case TipePenerima.Karyawan:
                    var karyawan = await _db.Karyawans.Include(k => k.Department).FirstOrDefaultAsync(k => k.Id == targetId);
                    if (karyawan != null)
                    {
                        ViewBag.NamaPenerima = karyawan.NamaLengkap;
                        ViewBag.KodePenerima = karyawan.Nik ?? "EMP-001";
                        ViewBag.EmailPenerima = karyawan.Email ?? "-";
                        ViewBag.TeleponPenerima = karyawan.NoTelepon ?? "-";
                        ViewBag.DivisiPenerima = karyawan.Department?.Name ?? "-";
                    }
                    break;

                case TipePenerima.Departemen:
                    var dept = await _db.Departments.FirstOrDefaultAsync(d => d.Id == targetId);
                    if (dept != null)
                    {
                        ViewBag.NamaPenerima = dept.Name;
                        ViewBag.KodePenerima = dept.Code ?? "DPT-001";
                        ViewBag.EmailPenerima = "-";
                        ViewBag.TeleponPenerima = "-";
                        ViewBag.DivisiPenerima = dept.Name;
                    }
                    break;

                case TipePenerima.Customer:
                    var cust = await _db.Customers.FirstOrDefaultAsync(c => c.Id == targetId);
                    if (cust != null)
                    {
                        ViewBag.NamaPenerima = cust.NamaCustomer;
                        ViewBag.KodePenerima = cust.KodeCustomer ?? "CUST-001";
                        ViewBag.EmailPenerima = cust.Email ?? "-";
                        ViewBag.TeleponPenerima = cust.NoTelepon ?? "-";
                        ViewBag.Alamat = cust.Alamat;
                    }
                    break;
            }
        }

        public async Task<PagedResultPenugasanAset<PenugasanAsetIndexViewModel>> GetPagedAsync(string? search, int? status, int pageIndex, int pageSize)
        {
            var query = _db.PenugasanAsets.AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim().ToLower();
                query = query.Where(p => p.KodePenugasan.ToLower().Contains(search) ||
                                         (p.Keterangan != null && p.Keterangan.ToLower().Contains(search)));
            }

            if (status.HasValue && status.Value > 0)
            {
                query = query.Where(p => (int)p.Status == status.Value);
            }

            int totalItems = await query.CountAsync();

            var rawData = await query.OrderByDescending(p => p.Id)
                                     .Skip((pageIndex - 1) * pageSize)
                                     .Take(pageSize)
                                     .ToListAsync();

            var items = new List<PenugasanAsetIndexViewModel>();

            foreach (var item in rawData)
            {
                string targetNama = "-";
                if (item.TipePenerima == TipePenerima.Karyawan)
                    targetNama = await _db.Karyawans.Where(k => k.Id == item.TargetPenerimaId).Select(k => k.NamaLengkap).FirstOrDefaultAsync() ?? "-";
                else if (item.TipePenerima == TipePenerima.Departemen)
                    targetNama = await _db.Departments.Where(d => d.Id == item.TargetPenerimaId).Select(d => d.Name).FirstOrDefaultAsync() ?? "-";
                else if (item.TipePenerima == TipePenerima.Customer)
                    targetNama = await _db.Customers.Where(c => c.Id == item.TargetPenerimaId).Select(c => c.NamaCustomer).FirstOrDefaultAsync() ?? "-";

                // Deteksi jika merupakan hasil Transfer Aset untuk info siklus A -> B
                string cycleInfo = string.Empty;
                if (item.Status == StatusPenugasan.TransferAsetLangsung && !string.IsNullOrEmpty(item.Keterangan) && item.Keterangan.Contains("[Transfer]"))
                {
                    cycleInfo = item.Keterangan;
                }

                items.Add(new PenugasanAsetIndexViewModel
                {
                    Id = item.Id,
                    KodePenugasan = item.KodePenugasan,
                    NamaTarget = targetNama,
                    TipeTargetStr = item.TipePenerima.ToString(),
                    TanggalSerahTerima = item.TanggalSerahTerima,
                    TanggalPengembalian = item.TanggalPengembalian,
                    Status = item.Status,
                    Keterangan = cycleInfo
                });
            }

            return new PagedResultPenugasanAset<PenugasanAsetIndexViewModel>
            {
                Items = items,
                TotalItems = totalItems,
                PageIndex = pageIndex,
                PageSize = pageSize
            };
        }

        public async Task<PenugasanAset?> GetByIdAsync(int id)
        {
            return await _db.PenugasanAsets.Include(p => p.Details).FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<PenugasanAsetFormViewModel?> GetViewModelByIdAsync(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity == null) return null;

            return new PenugasanAsetFormViewModel
            {
                Id = entity.Id,
                KodePenugasan = entity.KodePenugasan,
                TipePenerima = entity.TipePenerima,
                TargetPenerimaId = entity.TargetPenerimaId,
                TanggalSerahTerima = entity.TanggalSerahTerima,
                Keterangan = entity.Keterangan,
                Details = entity.Details.Select(d => new PenugasanAsetDetailViewModel
                {
                    Id = d.Id,
                    NamaBarang = d.NamaBarang,
                    SerialNumber = d.SerialNumber,
                    LokasiAwal = d.LokasiAwal,
                    Kuantitas = d.Kuantitas,
                    KondisiSerah = d.KondisiSerah,
                    KondisiKembali = d.KondisiKembali
                }).ToList()
            };
        }

        // GET: /PenugasanAset/GetAssignedAssets
        [HttpGet]
        public async Task<IActionResult> GetAssignedAssets(string? search)
        {
            var data = await _repo.GetAssignedAssetsAsync(search);
            return Json(new { success = true, data });
        }

        // GET: /PenugasanAset/GetAssignedAssetDetail
        [HttpGet]
        public async Task<IActionResult> GetAssignedAssetDetail(int id)
        {
            var detail = await _repo.GetAssignedAssetDetailAsync(id);
            if (detail == null) return Json(new { success = false, message = "Detail aset tidak ditemukan." });

            return Json(new { success = true, data = detail });
        }

        // POST: /PenugasanAset/ProcessTransferAsset
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProcessTransferAsset([FromForm] TransferAssetRequestDto dto)
        {
            if (dto.ProdukItemId <= 0 || dto.TargetPenerimaId <= 0)
            {
                return Json(new { success = false, message = "Pilih Aset dan Penerima Baru dengan benar!" });
            }

            try
            {
                await _repo.ProcessTransferAssetAsync(dto);
                return Json(new { success = true, message = "Proses Transfer Aset Langsung Berhasil Disimpan!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Gagal memproses transfer: " + ex.Message });
            }
        }

        // APPROVE: MenungguPersetujuan -> Approve => produk_item menjadi "DalamPenugasan"
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ApprovePenugasan(int id)
        {
            if (id <= 0) return Json(new { success = false, message = "ID tidak valid." });

            try
            {
                await _repo.ApprovePenugasanAsync(id);
                return Json(new { success = true, message = "Penugasan berhasil disetujui!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Gagal menyetujui: " + ex.Message });
            }
        }

        // REJECT: MenungguPersetujuan -> Ditolak => produk_item tetap / kembali "In Stock"
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RejectPenugasan(int id)
        {
            if (id <= 0) return Json(new { success = false, message = "ID tidak valid." });

            try
            {
                await _repo.RejectPenugasanAsync(id);
                return Json(new { success = true, message = "Penugasan berhasil ditolak." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Gagal menolak: " + ex.Message });
            }
        }

        // GET: /PenugasanAset/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var data = await _repo.GetByIdAsync(id);
            if (data == null)
            {
                TempData["ErrorMessage"] = "Data penugasan aset tidak ditemukan!";
                return RedirectToAction(nameof(Index));
            }

            // Load data relasi penerima lengkap ke ViewBag
            await PopulatePenerimaInfoToViewBag(data.TipePenerima, data.TargetPenerimaId);

            // Deteksi berdasarkan TipePenerima Enum
            switch (data.TipePenerima)
            {
                case TipePenerima.Karyawan:
                    return View("DetailsKaryawan", data);

                case TipePenerima.Departemen:
                    return View("DetailsDepartemen", data);

                case TipePenerima.Customer:
                    return View("DetailsCustomer", data);

                default:
                    TempData["ErrorMessage"] = "Tipe penerima pada penugasan ini tidak valid atau tidak dikenali.";
                    return RedirectToAction(nameof(Index));
            }
        }

    }
}