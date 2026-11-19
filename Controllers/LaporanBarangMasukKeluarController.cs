using InventoryMultiGudang.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryMultiGudang.Controllers
{
    //[Authorize(Roles = "Super Admin, Manager, User, Warehouse Manager,Warehouse Staff, Admin")]
    [Authorize(Policy = "view stock in/out reports")]
    public class LaporanBarangMasukKeluarController : Controller
    {
        private readonly ILaporanBarangMasukKeluarRepository _repository;

        public LaporanBarangMasukKeluarController(ILaporanBarangMasukKeluarRepository repository)
        {
            _repository = repository;
        }

        // Method Index untuk menampilkan halaman utama UI
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                var model = await _repository.GetIndexDataAsync(null, null, null, null, null);
                return View(model);
            }
            catch (Exception ex)
            {
                // Fallback error jika terjadi kendala pada server/database
                TempData["ErrorMessage"] = "Gagal memuat data laporan: " + ex.Message;
                return View(new ViewModels.LaporanBarangMasukKeluarIndexViewModel());
            }
        }

        // Method AJAX untuk filter data tabel secara dinamis tanpa reload halaman
        [HttpGet]
        public async Task<IActionResult> FilterData(string tanggalMulai, string tanggalSelesai, int? gudangId, string tipeTransaksi, int? produkId)
        {
            try
            {
                var data = await _repository.GetIndexDataAsync(tanggalMulai, tanggalSelesai, gudangId, tipeTransaksi, produkId);
                return Json(new { success = true, data = data });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Terjadi kesalahan server: " + ex.Message });
            }
        }

        // Method AJAX untuk pencarian data dinamis Select Gudang (Maksimal 5 data + Live Search)
        [HttpGet]
        public async Task<IActionResult> GetGudangList(string q)
        {
            try
            {
                var result = await _repository.GetGudangDropdownAsync(q);
                return Json(result);
            }
            catch (Exception)
            {
                return NotFound(new { message = "Data gudang tidak ditemukan." });
            }
        }

        // Method AJAX untuk pencarian data dinamis Select Produk (Maksimal 5 data + Live Search)
        [HttpGet]
        public async Task<IActionResult> GetProdukList(string q, int? gudangId)
        {
            try
            {
                var result = await _repository.GetProdukDropdownAsync(q, gudangId);
                return Json(result);
            }
            catch (Exception)
            {
                return NotFound(new { message = "Data produk tidak ditemukan." });
            }
        }

        // Contoh Action tambahan dengan respon JSON standar sukses untuk integrasi fitur CRUD masa mendatang
        [HttpPost]
        public IActionResult SimpanData()
        {
            try
            {
                // Proses penyimpanan data ke database...
                return Json(new { success = true, message = "Berhasil disimpan !" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = "Gagal menyimpan data: " + ex.Message });
            }
        }

        [HttpPost]
        public IActionResult HapusData(int id)
        {
            try
            {
                // Proses penghapusan data dari database...
                return Json(new { success = true, message = "Berhasil dihapus !" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = "Gagal menghapus data: " + ex.Message });
            }
        }

        // Endpoint Cetak Laporan (Langsung cetak saat tombol ditekan)
        [HttpGet]
        public async Task<IActionResult> Report(string tanggalMulai, string tanggalSelesai, int? gudangId, string tipeTransaksi, int? produkId)
        {
            try
            {
                var model = await _repository.GetIndexDataAsync(tanggalMulai, tanggalSelesai, gudangId, tipeTransaksi, produkId);

                // Debug & Set Periode untuk Header Cetak
                if (!string.IsNullOrEmpty(tanggalMulai) && !string.IsNullOrEmpty(tanggalSelesai))
                {
                    ViewBag.Periode = $"{tanggalMulai} s/d {tanggalSelesai}";
                }
                else if (!string.IsNullOrEmpty(tanggalMulai))
                {
                    ViewBag.Periode = $"Mulai {tanggalMulai}";
                }
                else if (!string.IsNullOrEmpty(tanggalSelesai))
                {
                    ViewBag.Periode = $"s/d {tanggalSelesai}";
                }
                else
                {
                    ViewBag.Periode = "Semua Periode";
                }

                if (string.IsNullOrEmpty(tipeTransaksi))
                {
                    ViewBag.TipeTransaksi = "Semua Tipe";
                }
                else if (tipeTransaksi.ToLower() == "in")
                {
                    ViewBag.TipeTransaksi = "MASUK (STOCK IN)";
                }
                else if (tipeTransaksi.ToLower() == "out")
                {
                    ViewBag.TipeTransaksi = "KELUAR (STOCK OUT)";
                }
                else
                {
                    ViewBag.TipeTransaksi = tipeTransaksi;
                }

                if (gudangId.HasValue && gudangId.Value > 0)
                {
                    var gudangList = await _repository.GetGudangDropdownAsync("");
                    var selectedGudang = gudangList.FirstOrDefault(g => g.Id == gudangId.Value);
                    ViewBag.NamaGudang = selectedGudang != null ? selectedGudang.Text : "Semua Gudang";
                }
                else
                {
                    ViewBag.NamaGudang = "Semua Gudang";
                }

                return View(model);
            }
            catch (Exception)
            {
                return NotFound();
            }
        }

    }
}