using System.Diagnostics;
using InventoryMultiGudang.Models;
using Microsoft.AspNetCore.Mvc;

namespace InventoryMultiGudang.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        // Endpoint khusus untuk cron-job.org
        [HttpGet("/ping-endpoint")]
        public IActionResult Ping()
        {
            _logger.LogInformation("Web diping oleh cron-job.org pada: {Time}", System.DateTime.Now);
            // Mengembalikan respon teks sederhana 
            return Content("Aplikasi Aktif!", "text/plain");
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
