using InventoryMultiGudang.Models;
using InventoryMultiGudang.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InventoryMultiGudang.Controllers
{
    //[Authorize(Roles = "Super Admin, Manager, User, Warehouse Manager,Warehouse Staff, Admin")]
    [Authorize(Policy = "view asset reports")]
    public class LaporanAssetController : Controller
    {
        private readonly ILaporanAssetRepository _repository;
        private readonly ApplicationDbContext _context;

        public LaporanAssetController(ILaporanAssetRepository repository, ApplicationDbContext context)
        {
            _repository = repository;
            _context = context;
        }
        public async Task<IActionResult> IndexAsync()
        {
            var model = await _repository.GetIndexSummaryAsync();
            return View(model);
        }

        // Endpoint Ajax untuk memuat data tabel & pagination secara dinamis
        [HttpGet]
        public async Task<IActionResult> GetSerialNumbers(string search = "", int page = 1, int pageSize = 15)
        {
            var (items, totalCount) = await _repository.GetDaftarSerialNumberAsync(search, page, pageSize);

            var result = new
            {
                items = items,
                totalCount = totalCount,
                currentPage = page,
                pageSize = pageSize,
                totalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
            };

            return Json(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetPeminjamanBelumKembaliData(string search = "", int page = 1, int pageSize = 15)
        {
            var (items, totalCount) = await _repository.GetPeminjamanBelumKembaliAsync(search, page, pageSize);

            var result = new
            {
                items = items,
                totalCount = totalCount,
                currentPage = page,
                pageSize = pageSize,
                totalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
            };

            return Json(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAssetPerCustomerData(string search = "", int page = 1, int pageSize = 15)
        {
            var (items, totalCount) = await _repository.GetAssetPerCustomerAsync(search, page, pageSize);

            var result = new
            {
                items = items,
                totalCount = totalCount,
                currentPage = page,
                pageSize = pageSize,
                totalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
            };

            return Json(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetCustomerAssetDetails(int customerId)
        {
            var details = await _repository.GetDetailAssetByCustomerAsync(customerId);
            return Json(details);
        }

        [HttpGet]
        public async Task<IActionResult> GetAssetPerDepartemenData(string search = "", int page = 1, int pageSize = 15)
        {
            var (items, totalCount) = await _repository.GetAssetPerDepartemenAsync(search, page, pageSize);

            var result = new
            {
                items = items,
                totalCount = totalCount,
                currentPage = page,
                pageSize = pageSize,
                totalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
            };

            return Json(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetDepartemenAssetDetails(int departmentId)
        {
            var details = await _repository.GetDetailAssetByDepartemenAsync(departmentId);
            return Json(details);
        }

        [HttpGet]
        public async Task<IActionResult> GetAssetPerKaryawanData(string search = "", int page = 1, int pageSize = 15)
        {
            var (items, totalCount) = await _repository.GetAssetPerKaryawanAsync(search, page, pageSize);

            var result = new
            {
                items = items,
                totalCount = totalCount,
                currentPage = page,
                pageSize = pageSize,
                totalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
            };

            return Json(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetKaryawanAssetDetails(int karyawanId)
        {
            var details = await _repository.GetDetailAssetByKaryawanAsync(karyawanId);
            return Json(details);
        }

        [HttpGet]
        public async Task<IActionResult> Details(string sn)
        {
            if (string.IsNullOrEmpty(sn))
            {
                // Default fallback jika tidak ada parameter SN yang dikirim
                sn = "FAB-0001-GDG-SUB-02-0001";
            }

            var model = await _repository.GetAssetTimelineBySerialNumberAsync(sn);
            if (model == null)
            {
                return NotFound();
            }

            return View(model);
        }
        // Endpoint helper untuk dropdown pilih serial number aset
        [HttpGet]
        public async Task<IActionResult> GetProductItemDropdown()
        {
            var list = await _context.ProdukItems
                .Select(x => new { x.Id, Text = x.SerialNumber })
                .ToListAsync();
            return Json(list);
        }

        public IActionResult Details() => View();
        public IActionResult AssetDaftarSerialNumber() => View();
        public IActionResult AssetPeminjamanBelumKembali() => View();
        public IActionResult AssetPerCustumer() => View();
        public IActionResult AssetPerDepartemen() => View();
        public IActionResult AssetPerKaryawan() => View();

    }
}
