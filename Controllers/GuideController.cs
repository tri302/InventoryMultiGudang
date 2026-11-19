using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryMultiGudang.Controllers
{
    //[Authorize(Roles = "Super Admin, Manager, User, Warehouse Manager,Warehouse Staff, Admin")]
    [Authorize(Policy = "view user guide")]
    public class GuideController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }                
        public IActionResult Pengenalan()
        {
            return View();
        }
        public IActionResult KonsepDasarSistem()
        {
            return View();
        }
        public IActionResult TransaksiBarangMasuk()
        {
            return View();
        }
        public IActionResult TransaksiBarangKeluar()
        {
            return View();
        }
        public IActionResult Mutasi()
        {
            return View();
        }
        public IActionResult PersetujuanPenugasanAset()
        {
            return View();
        }
        public IActionResult PeminjamanAsetOverdue()
        {
            return View();
        }
        public IActionResult Faqs()
        {
            return View();
        }
        public IActionResult Troubleshooting()
        {
            return View();
        }
    }
}
