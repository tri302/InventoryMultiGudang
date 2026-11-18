using Microsoft.AspNetCore.Mvc;

namespace InventoryMultiGudang.Controllers
{
    public class MaintenanceController : Controller
    {
        public IActionResult Index()
        {
            return View("~/Views/Maintenance/Maintenance.cshtml");
    }
    }
}
