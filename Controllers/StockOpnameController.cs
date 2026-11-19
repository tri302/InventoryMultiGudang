using InventoryMultiGudang.Models;
using InventoryMultiGudang.Repository;
using InventoryMultiGudang.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace InventoryMultiGudang.Controllers
{
    //[Authorize(Roles = "Super Admin, Manager, User, Warehouse Manager,Warehouse Staff, Admin")]
    [Authorize(Policy = "manage stock opname")]
    public class StockOpnameController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IStockOpnameRepository _repository;

        public StockOpnameController(ApplicationDbContext context, IStockOpnameRepository repository)
        {
            _context = context;
            _repository = repository;
        }

        // GET: /StockOpname/Index
        public async Task<IActionResult> Index()
        {
            // Populasi dropdown gudang dari DB secara dinamis
            var listGudang = await _repository.GetAllGudangAsync();
            ViewBag.GudangList = new SelectList(listGudang, "Id", "NamaGudang");
            return View();
        }

        // GET: /StockOpname/GetPagedData (Fitur Search & Pagination Ajax)
        [HttpGet]
        public async Task<IActionResult> GetPagedData(string? search, int? gudangId, string? status, int page = 1, int pageSize = 5)
        {
            var (data, totalCount) = await _repository.GetPagedAsync(search, gudangId, status, page, pageSize);

            var result = new PagedResultStockOpname<StockOpnameListViewModel>
            {
                Items = data.ToList(),
                TotalItems = totalCount,
                PageNumber = page,
                PageSize = pageSize
            };

            return Json(new { success = true, data = result });
        }

        // GET: /StockOpname/GetGudangList
        [HttpGet]
        public async Task<IActionResult> GetGudangList()
        {
            var gudangList = await _repository.GetAllGudangAsync();
            return Json(new { success = true, data = gudangList });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> InitAudit([FromBody] TargetAuditViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, message = "Silakan pilih gudang target audit terlebih dahulu!" });
            }

            TempData["SelectedGudangId"] = model.GudangId;
            TempData.Keep("SelectedGudangId"); // Menjaga TempData tetap ada untuk request selanjutnya

            var gudang = await _context.Gudangs.FindAsync(model.GudangId);
            if (gudang != null)
            {
                gudang.IsAudit = true;
                _context.Gudangs.Update(gudang);
                await _context.SaveChangesAsync();
            }

            return Json(new { success = true, redirectUrl = Url.Action("AuditStockOpname") });
        }

        // GET: /StockOpname/AuditStockOpname
        public async Task<IActionResult> AuditStockOpname(int? id)
        {
            StockOpnameFormViewModel vm = new StockOpnameFormViewModel();

            if (id.HasValue && id.Value > 0)
            {
                // ==========================================
                // 1. MODE EDIT DRAF
                // ==========================================   
                // 1. Ambil username atau identitas user yang sedang aktif login
                string? currentUser = User.Identity?.Name;
                // 2. Konversi username login menjadi Nama Lengkap Karyawan menggunakan helper repository
                string penyerah = await _repository.GetNamaPenyerahAsync(currentUser);
                // 3. Panggil GetByIdAsync dengan menyertakan nama penyerah yang sudah valid
                var audit = await _repository.GetByIdAsync(id.Value, penyerah);                             
                if (audit == null) return NotFound();

                vm.Id = audit.Id;
                vm.NoReferensi = audit.NoReferensi;
                vm.GudangId = audit.GudangId;
                vm.NamaGudang = audit.Gudang?.NamaGudang;
                vm.KodeGudang = audit.Gudang?.KodeGudang;
                vm.Catatan = audit.Catatan;

                vm.Items = new List<StockOpnameDetailViewModel>();
                var groupedDetails = audit.Details.GroupBy(d => d.ProdukId);

                foreach (var group in groupedDetails)
                {
                    var firstDetail = group.First();
                    var produk = firstDetail.Produk;
                    bool isSn = produk.PelacakanBarang != null && produk.PelacakanBarang.Equals("Pelacakan Serial Number (SN)", StringComparison.OrdinalIgnoreCase);

                    // PERBAIKAN DI SINI: Jangan batasi hanya "In Stock", ambil semua yang aktif (kecuali Lost/Disposed)
                    var produkItemsFromDb = isSn
                        ? await _context.ProdukItems
                            .Where(pi => pi.ProdukId == group.Key && pi.GudangId == audit.GudangId && pi.Status != "Lost" && pi.Status != "Disposed")
                            .ToListAsync()
                        : new List<ProdukItem>();

                    int calculatedStokFisik = produkItemsFromDb.Count > 0
                        ? produkItemsFromDb.Count(pi => (group.FirstOrDefault(g => g.ProdukItemId == pi.Id)?.Kondisi ?? pi.Status ?? "Ditemukan (OK)") != "Hilang (Lost)")
                        : firstDetail.StokFisik;
                    int stokSistem = firstDetail.Produk?.Stok ?? 0;
                    int calculatedSelisih = calculatedStokFisik - stokSistem;

                    vm.Items.Add(new StockOpnameDetailViewModel
                    {
                        Id = firstDetail.Id,
                        ProdukId = group.Key,
                        NamaProduk = produk?.NamaProduk ?? firstDetail.Produk?.NamaProduk,
                        Sku = produk?.Sku ?? firstDetail.Produk?.Sku ?? string.Empty,
                        TipeBarang = produk?.TipeBarang ?? firstDetail.Produk?.TipeBarang,
                        StokSistem = stokSistem,
                        StokFisik = calculatedStokFisik,
                        AlasanSelisih = firstDetail.AlasanSelisih,
                        Keterangan = firstDetail.Keterangan,
                        SnItems = produkItemsFromDb.Select(pi => new StockOpnameViewModel
                        {
                            ProdukItemId = pi.Id,
                            SerialNumber = pi.SerialNumber,
                            // Mengambil kondisi draf yang tersimpan atau status aktual sistem (Assigned/Loaned/Repair/dll)
                            Kondisi = group.FirstOrDefault(g => g.ProdukItemId == pi.Id)?.Kondisi ?? pi.Status ?? "Ditemukan (OK)"
                        }).ToList()
                    });
                }
            }
            else
            {
                // ==========================================
                // 2. MODE BARU (NEW FORM)
                // ==========================================                
                // Gunakan Peek agar TempData tidak terhapus
                int gudangId = TempData.Peek("SelectedGudangId") != null
                    ? Convert.ToInt32(TempData.Peek("SelectedGudangId"))
                    : 1;

                // Ambil gudang target langsung dari database tanpa filter IsAudit
                var gudang = await _context.Gudangs.FindAsync(gudangId);
                int targetGudangId = gudang?.Id ?? 1;
                var produks = await _repository.GetProdukByGudangIdAsync(targetGudangId);
                vm.GudangId = targetGudangId;
                vm.NamaGudang = gudang?.NamaGudang ?? "Gudang Utama Jakarta";
                vm.KodeGudang = gudang?.KodeGudang ?? "GDG-JKT-01";
                vm.NoReferensi = await _repository.GenerateNoReferensiAsync();
                vm.Items = new List<StockOpnameDetailViewModel>();
                foreach (var p in produks)
                {
                    bool isSn = p.PelacakanBarang != null && p.PelacakanBarang.Contains("Pelacakan Serial Number (SN)", StringComparison.OrdinalIgnoreCase);
                    // Ambil item khusus di gudang target yang statusnya tidak Lost/Disposed
                    var produkItemsFromDb = isSn
                        ? await _context.ProdukItems
                            .Where(pi => pi.ProdukId == p.Id && pi.GudangId == targetGudangId && pi.Status != "Lost" && pi.Status != "Disposed")
                            .ToListAsync()
                        : new List<ProdukItem>();

                    // Jika produk ber-SN tetapi tidak ada satupun item aktif di gudang ini, lewati
                    if (isSn && produkItemsFromDb.Count == 0)
                    {
                        continue;
                    }
                    // Untuk produk Non-SN, pastikan produk tersebut memang milik/tersedia untuk gudang target ini
                    if (!isSn && p.GudangId != targetGudangId)
                    {
                        continue;
                    }
                    vm.Items.Add(new StockOpnameDetailViewModel
                    {
                        ProdukId = p.Id,
                        NamaProduk = p.NamaProduk,
                        Sku = p.Sku,
                        TipeBarang = p.TipeBarang,
                        StokSistem = produkItemsFromDb.Count > 0 ? produkItemsFromDb.Count : p.Stok,
                        StokFisik = produkItemsFromDb.Count > 0 ? produkItemsFromDb.Count : p.Stok,
                        SnItems = produkItemsFromDb.Select(pi => new StockOpnameViewModel
                        {
                            ProdukItemId = pi.Id,
                            SerialNumber = pi.SerialNumber,
                            Kondisi = pi.Status ?? "Ditemukan (OK)",
                            AlasanSelisih = pi.AlasanSelisih,
                            Keterangan = pi.Keterangan
                        }).ToList()
                    });
                }
            }
            return View(vm);
        }

        // POST: /StockOpname/SaveDraft
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SaveDraft([FromBody] StockOpnameFormViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                return Json(new { success = false, message = "Validasi gagal: " + string.Join(", ", errors) });
            }

            try
            {
                // Helper function yang menerima parameter isSnTracked dengan benar
                (string kondisi, int stokFisik) GetKondisiDanStokFisik(string itemKondisi, string itemStatus, int inputStokFisik, bool isSnTracked)
                {
                    string kondisiFinal = !string.IsNullOrEmpty(itemKondisi) ? itemKondisi : (!string.IsNullOrEmpty(itemStatus) ? itemStatus : "Ditemukan (OK)");
                    int fisikFinal;
                    if (isSnTracked)
                    {
                        // Jika ber-SN, nilai fisik menjadi 0 jika Hilang atau Dihapuskan
                        fisikFinal = (kondisiFinal == "Hilang (Lost)" || kondisiFinal == "Dihapuskan (Disposed)") ? 0 : 1;
                    }
                    else
                    {
                        // Jika Non-SN, mutlak ambil angka inputan manual dari form (misal: 9)
                        fisikFinal = inputStokFisik;
                    }
                    return (kondisiFinal, fisikFinal);
                }

                if (model.Id > 0)
                {
                    var existing = await _repository.GetByIdAsync(model.Id);
                    if (existing == null) return Json(new { success = false, message = "Data tidak ditemukan!" });

                    existing.Catatan = model.Catatan;
                    existing.Details.Clear();

                    foreach (var item in model.Items)
                    {
                        var produk = await _repository.GetProdukByIdAsync(item.ProdukId);
                        int stokSistemDariDb = produk != null ? produk.Stok : 0;

                        // Cek apakah produk ini ber-SN atau Non-SN secara presisi
                        bool isSn = produk != null && produk.PelacakanBarang != null &&
                                    produk.PelacakanBarang.Contains("Pelacakan Serial Number (SN)", StringComparison.OrdinalIgnoreCase);

                        // Kirim status isSn yang valid ke fungsi helper
                        var (kondisi, stokFisik) = GetKondisiDanStokFisik(item.Kondisi, item.Status, item.StokFisik, isSn);

                        int? validProdukItemId = null;
                        if (item.ProdukItemId > 0)
                        {
                            bool itemExists = await _context.ProdukItems
                                .AnyAsync(pi => pi.Id == item.ProdukItemId && pi.ProdukId == item.ProdukId);
                            if (itemExists)
                            {
                                validProdukItemId = item.ProdukItemId;
                            }
                        }

                        var detail = new StockOpnameDetail
                        {
                            ProdukId = item.ProdukId,
                            ProdukItemId = validProdukItemId,
                            StokSistem = stokSistemDariDb,
                            Kondisi = kondisi,
                            StokFisik = stokFisik, // Menyimpan stok fisik manual untuk Non-SN atau hasil hitung SN dengan benar
                            AlasanSelisih = item.AlasanSelisih,
                            Keterangan = item.Keterangan,
                            SerialNumber = item.SerialNumber
                        };

                        existing.Details.Add(detail);
                    }

                    await _repository.UpdateDraftAsync(existing);
                }
                else
                {
                    var opname = new StockOpname
                    {
                        NoReferensi = await _repository.GenerateNoReferensiAsync(),
                        GudangId = model.GudangId,
                        Catatan = model.Catatan,
                        Auditor = "Super Admin"
                    };

                    foreach (var item in model.Items)
                    {
                        var produk = await _repository.GetProdukByIdAsync(item.ProdukId);
                        int stokSistemDariDb = produk != null ? produk.Stok : 0;

                        // Cek apakah produk ini ber-SN atau Non-SN secara presisi
                        bool isSn = produk != null && produk.PelacakanBarang != null &&
                                    produk.PelacakanBarang.Contains("Pelacakan Serial Number (SN)", StringComparison.OrdinalIgnoreCase);

                        // Kirim status isSn yang valid ke fungsi helper
                        var (kondisi, stokFisik) = GetKondisiDanStokFisik(item.Kondisi, item.Status, item.StokFisik, isSn);

                        int? validProdukItemId = null;
                        if (item.ProdukItemId > 0)
                        {
                            bool itemExists = await _context.ProdukItems
                                .AnyAsync(pi => pi.Id == item.ProdukItemId && pi.ProdukId == item.ProdukId);
                            if (itemExists)
                            {
                                validProdukItemId = item.ProdukItemId;
                            }
                        }

                        var detail = new StockOpnameDetail
                        {
                            ProdukId = item.ProdukId,
                            ProdukItemId = validProdukItemId,
                            StokSistem = stokSistemDariDb,
                            Kondisi = kondisi,
                            StokFisik = stokFisik, // Menyimpan stok fisik manual untuk Non-SN atau hasil hitung SN dengan benar
                            AlasanSelisih = item.AlasanSelisih,
                            Keterangan = item.Keterangan,
                            SerialNumber = item.SerialNumber
                        };

                        opname.Details.Add(detail);
                    }
                    await _repository.CreateDraftAsync(opname);
                }

                return Json(new { success = true, message = "Berhasil disimpan !", redirectUrl = Url.Action("Index") });
            }
            catch (Exception ex)
            {
                string errorMessage = ex.Message;
                if (ex.InnerException != null)
                {
                    errorMessage += " | Inner Exception: " + ex.InnerException.Message;
                }

                return Json(new { success = false, message = "Terjadi kesalahan: " + errorMessage });
            }
        }

        // GET: /StockOpname/Details/5
        public async Task<IActionResult> Details(int id)
        {
            StockOpnameFormViewModel vm = new StockOpnameFormViewModel();

            if (id != 0 && id > 0)
            {
                // 1. Ambil username atau identitas user yang sedang aktif login
                string? currentUser = User.Identity?.Name;
                // 2. Konversi username login menjadi Nama Lengkap Karyawan menggunakan helper repository
                string penyerah = await _repository.GetNamaPenyerahAsync(currentUser);
                // 3. Panggil GetByIdAsync dengan menyertakan nama penyerah yang sudah valid
                var audit = await _repository.GetByIdAsync(id, penyerah);
                if (audit == null) return NotFound();

                vm.Id = audit.Id;
                vm.NoReferensi = audit.NoReferensi;
                vm.GudangId = audit.GudangId;
                vm.NamaGudang = audit.Gudang?.NamaGudang;
                vm.KodeGudang = audit.Gudang?.KodeGudang;
                vm.Catatan = audit.Catatan;
                vm.TanggalMulai = audit.TanggalMulai;
                vm.TanggalSelesai = audit.TanggalSelesai;
                vm.Status = audit.Status;

                vm.Auditor = audit.Auditor ?? "Admin Gudang";

                vm.Items = new List<StockOpnameDetailViewModel>();
                var groupedDetails = audit.Details.GroupBy(d => d.ProdukId);
                foreach (var group in groupedDetails)
                {
                    var firstDetail = group.First();

                    // Murni ambil dari daftar item SN historis di detail opname
                    var snItemsList = group.Where(g => !string.IsNullOrEmpty(g.SerialNumber)).Select(g => new StockOpnameViewModel
                    {
                        ProdukItemId = g.ProdukItemId ?? 0,
                        SerialNumber = g.SerialNumber,
                        // Pastikan kondisi/status terbaca dengan baik
                        Kondisi = g.Kondisi ?? "Ditemukan (OK)",
                        AlasanSelisih = g.AlasanSelisih,
                        Keterangan = g.Keterangan
                    }).ToList();

                    int calculatedStokFisik = snItemsList.Count > 0
                        ? snItemsList.Count(x => x.Kondisi != "Hilang (Lost)" && x.Kondisi != "Dihapuskan (Disposed)")
                        : firstDetail.StokFisik;

                    int stokSistem = firstDetail.StokSistem;

                    vm.Items.Add(new StockOpnameDetailViewModel
                    {
                        Id = firstDetail.Id,
                        ProdukId = group.Key,
                        NamaProduk = firstDetail.Produk?.NamaProduk,
                        Sku = firstDetail.Produk?.Sku ?? string.Empty,
                        TipeBarang = firstDetail.Produk?.TipeBarang,
                        StokSistem = stokSistem,
                        StokFisik = calculatedStokFisik,
                        SnItems = snItemsList,
                        AlasanSelisih = firstDetail.AlasanSelisih,
                        Keterangan = firstDetail.Keterangan
                    });
                }
            }
            return View(vm);
        }

        // GET: /StockOpname/Print/5
        public async Task<IActionResult> Print(int id)
        {
            StockOpnameFormViewModel vm = new StockOpnameFormViewModel();
            if (id != 0 && id > 0)
            {
                // 1. Ambil username atau identitas user yang sedang aktif login
                string? currentUser = User.Identity?.Name;
                // 2. Konversi username login menjadi Nama Lengkap Karyawan menggunakan helper repository
                string penyerah = await _repository.GetNamaPenyerahAsync(currentUser);
                // 3. Panggil GetByIdAsync dengan menyertakan nama penyerah yang sudah valid
                var audit = await _repository.GetByIdAsync(id, penyerah);
                if (audit == null) return NotFound();

                vm.Id = audit.Id;
                vm.NoReferensi = audit.NoReferensi;
                vm.GudangId = audit.GudangId;
                vm.NamaGudang = audit.Gudang?.NamaGudang;
                vm.KodeGudang = audit.Gudang?.KodeGudang;
                vm.AlamatGudang = audit.Gudang?.AlamatGudang;
                vm.Catatan = audit.Catatan;
                vm.TanggalMulai = audit.TanggalMulai;
                vm.TanggalSelesai = audit.TanggalSelesai ?? DateTime.Now;
                vm.Status = audit.Status;
                vm.Auditor = audit.Auditor ?? "Admin Gudang";

                vm.Items = new List<StockOpnameDetailViewModel>();
                var groupedDetails = audit.Details.GroupBy(d => d.ProdukId);

                foreach (var group in groupedDetails)
                {
                    var firstDetail = group.First();

                    var snItemsList = group.Where(g => !string.IsNullOrEmpty(g.SerialNumber)).Select(g => new StockOpnameViewModel
                    {
                        ProdukItemId = g.ProdukItemId ?? 0,
                        SerialNumber = g.SerialNumber,
                        // Pastikan kondisi/status historis (termasuk Assigned, Loaned, Repair, dll) terbaca dengan benar
                        Kondisi = g.Kondisi ?? "Ditemukan (OK)",
                        AlasanSelisih = g.AlasanSelisih,
                        Keterangan = g.Keterangan
                    }).ToList();

                    // KUNCI: Hitung fisik dengan mengecualikan yang Hilang atau Disposed
                    int calculatedStokFisik = snItemsList.Count > 0
                        ? snItemsList.Count(x => x.Kondisi != "Hilang (Lost)" && x.Kondisi != "Dihapuskan (Disposed)")
                        : firstDetail.StokFisik;

                    int stokSistem = firstDetail.StokSistem;
                    bool isSnProduct = snItemsList != null && snItemsList.Count > 0;

                    vm.Items.Add(new StockOpnameDetailViewModel
                    {
                        Id = firstDetail.Id,
                        ProdukId = group.Key,
                        NamaProduk = firstDetail.Produk?.NamaProduk,
                        Sku = firstDetail.Produk?.Sku ?? string.Empty,
                        TipeBarang = firstDetail.Produk?.TipeBarang,
                        StokSistem = stokSistem,
                        StokFisik = calculatedStokFisik,
                        SnItems = snItemsList,
                        AlasanSelisih = isSnProduct ? null : firstDetail.AlasanSelisih,
                        Keterangan = isSnProduct ? null : firstDetail.Keterangan
                    });
                }
            }

            var setting = await _context.SystemSettings.FirstOrDefaultAsync();
            ViewBag.SiteName = setting?.SiteName ?? "PT. CONTOH JAYA UTAMA";
            ViewBag.SiteDescription = setting?.SiteDescription ?? "INVENTORY MULTI-GUDANG MODERN, AUDIT STOK PRESISI, DAN REKAPITULASI ASET REAL-TIME.";
            ViewBag.CompanyAddress = setting?.CompanyAddress ?? "Komp. Perkantoran Indah Blok B-12, Jakarta Selatan, Indonesia";
            ViewBag.CompanyPhone = setting?.CompanyPhone ?? "(021) 555-9830";
            ViewBag.CompanyEmail = setting?.CompanyEmail ?? "support@ptcontohjayautama.com";

            return View("Print", vm);
        }

        // POST: /StockOpname/SelesaikanAudit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SelesaikanAudit(int id)
        {
            try
            {
                bool result = await _repository.SelesaikanAuditAsync(id);
                if (result)
                {
                    return Json(new { success = true, message = "Berhasil diselesaikan dan disinkronisasi!", redirectUrl = Url.Action("Index") });
                }
                return Json(new { success = false, message = "Gagal menyelesaikan audit. Status dokumen mungkin bukan Draft." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Terjadi kesalahan: " + ex.Message });
            }
        }

        // POST: /StockOpname/BatalkanAudit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> BatalkanAudit(int id)
        {
            try
            {
                bool result = await _repository.BatalkanAuditAsync(id);
                if (result)
                {
                    return Json(new { success = true, message = "Audit berhasil dibatalkan!", redirectUrl = Url.Action("Index") });
                }
                return Json(new { success = false, message = "Gagal membatalkan audit." });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Terjadi kesalahan: " + ex.Message });
            }
        }

        // POST: /StockOpname/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                bool result = await _repository.DeleteAsync(id);
                if (result)
                {
                    return Json(new { success = true, message = "Berhasil dihapus !", redirectUrl = Url.Action("Index") });
                }
                return Json(new { success = false, message = "Data gagal dihapus atau tidak ditemukan!" });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Terjadi kesalahan: " + ex.Message });
            }
        }
    }
}
