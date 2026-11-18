using InventoryMultiGudang.Models;
using InventoryMultiGudang.Repository;
using InventoryMultiGudang.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryMultiGudang.Controllers
{
    //[Authorize(Roles = "Super Admin, Manager, Warehouse Manager")]
    [Authorize(Policy = "manage customer")]
    public class CustomerController : Controller
    {
        private readonly ICustomerRepository _customerRepo;
        private readonly IWebHostEnvironment _env;

        public CustomerController(ICustomerRepository customerRepo, IWebHostEnvironment env)
        {
            _customerRepo = customerRepo;
            _env = env;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetData(CustomerDataTableRequest request)
        {
            var data = await _customerRepo.GetPagedAsync(request);
            return PartialView("_CustomerTablePartial", data);
        }

        [HttpGet]
        public async Task<IActionResult> GetById(int id)
        {
            var customer = await _customerRepo.GetByIdAsync(id);
            if (customer == null)
                return Json(new { success = false, message = "Data customer tidak ditemukan!" });

            var vm = new CustomerViewModel
            {
                Id = customer.Id,
                KodeCustomer = customer.KodeCustomer,
                NamaCustomer = customer.NamaCustomer,
                Email = customer.Email,
                NoTelepon = customer.NoTelepon,
                Alamat = customer.Alamat,
                IsActive = customer.IsActive
            };

            return Json(new { success = true, data = vm });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CustomerViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return Json(new { success = false, message = "Validasi gagal!", errors });
            }

            var model = new Customer
            {
                KodeCustomer = await _customerRepo.GenerateKodeCustomerAsync(),
                NamaCustomer = vm.NamaCustomer,
                Email = vm.Email,
                NoTelepon = vm.NoTelepon,
                Alamat = vm.Alamat,
                IsActive = vm.IsActive
            };

            await _customerRepo.AddAsync(model);
            return Json(new { success = true, message = "Berhasil disimpan !" });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CustomerViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return Json(new { success = false, message = "Validasi gagal!", errors });
            }

            var customer = await _customerRepo.GetByIdAsync(vm.Id);
            if (customer == null)
                return Json(new { success = false, message = "Data customer tidak ditemukan!" });

            customer.NamaCustomer = vm.NamaCustomer;
            customer.Email = vm.Email;
            customer.NoTelepon = vm.NoTelepon;
            customer.Alamat = vm.Alamat;
            customer.IsActive = vm.IsActive;            

            await _customerRepo.UpdateAsync(customer);
            return Json(new { success = true, message = "Berhasil disimpan !" });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var success = await _customerRepo.DeleteAsync(id);
            if (success)
            {
                return Json(new { success = true, message = "Berhasil dihapus !" });
            }
            return Json(new { success = false, message = "Gagal menghapus data!" });
        }        
    }
}
