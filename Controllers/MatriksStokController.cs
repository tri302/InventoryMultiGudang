using InventoryMultiGudang.Models;
using InventoryMultiGudang.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryMultiGudang.Controllers
{
    //[Authorize(Roles = "Super Admin, Manager, User, Warehouse Manager,Warehouse Staff, Admin")]
    [Authorize(Policy = "view stock matrix")]
    public class MatriksStokController : Controller
    {
        private readonly IMatriksStokRepository _repository;

        public MatriksStokController(IMatriksStokRepository repository)
        {
            _repository = repository;
        }
        
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Endpoint AJAX untuk memuat data tabel dan pagination secara dinamis tanpa reload page.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetData(int? gudangId, int page = 1, int pageSize = 10)
        {
            try
            {
                var warehouses = await _repository.GetAllWarehousesAsync();
                var (products, totalCount) = await _repository.GetMatriksStokDataAsync(gudangId, page, pageSize);

                var viewModel = new MatriksStokIndexViewModel
                {
                    Warehouses = new List<WarehouseHeaderDto>(warehouses),
                    Products = new List<ProductStockRowDto>(products),
                    CurrentPage = page,
                    TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize),
                    TotalRecords = totalCount
                };

                return PartialView("_MatriksStokTablePartial", viewModel);
            }
            catch (Exception ex)
            {
                // Fallback error ke UI menggunakan format JSON pesan penanganan Toastr
                return StatusCode(500, new { success = false, message = "Gagal memuat data matriks stok: " + ex.Message });
            }
        }

        /// <summary>
        /// Endpoint AJAX untuk pencarian data gudang secara live search pada select element.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> SearchWarehouses(string q)
        {
            try
            {
                var warehouses = await _repository.GetAllWarehousesAsync(q, limit: 10);
                return Json(warehouses);
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = "Gagal mencari gudang: " + ex.Message });
            }
        }

        /// <summary>
        /// Endpoint untuk mencetak laporan Matriks Stok (Tab baru / Target Blank).
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> ReportMatrikStok()
        {
            try
            {
                // 1. Ambil username atau identitas user yang sedang aktif login
                string? currentUser = User.Identity?.Name;
                // 2. Konversi username login menjadi Nama Lengkap Karyawan menggunakan helper repository
                string penyerah = await _repository.GetNamaPenyerahAsync(currentUser);
                // 3. Panggil GetByIdAsync dengan menyertakan nama penyerah yang sudah valid            
                // Ambil data laporan langsung yang bertipe MatriksStokReportVm dari repository
                var reportModel = await _repository.GetLaporanMatriksStokAsync(penyerah);
                // Kembalikan view laporan dengan model yang sesuai
                return View("~/Views/MatriksStok/ReportMatrikStok.cshtml", reportModel);
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Gagal memuat data laporan matriks stok: " + ex.Message;
                // Fallback mengembalikan model kosong bertipe MatriksStokReportVm
                return View("~/Views/MatriksStok/ReportMatrikStok.cshtml", new InventoryMultiGudang.ViewModels.MatriksStokReportVm());
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateSafetyStock(int productId, int warehouseId, int safetyStock)
        {
            bool success = await _repository.UpdateSafetyStockAsync(productId, warehouseId, safetyStock);

            if (success)
            {
                return Json(new { success = true, message = "Safety stock berhasil diperbarui!" });
            }

            return BadRequest(new { success = false, message = "Gagal memperbarui safety stock." });
        }

    }
}