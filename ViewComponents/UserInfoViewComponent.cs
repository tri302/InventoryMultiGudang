using InventoryMultiGudang.Models;
using InventoryMultiGudang.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace InventoryMultiGudang.ViewComponents
{
    public class UserInfoViewComponent : ViewComponent
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserInfoViewComponent(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            // Pastikan user benar-benar terautentikasi
            if (User.Identity == null || !User.Identity.IsAuthenticated)
            {
                return Content(string.Empty);
            }

            // Ambil user beserta data Karyawan terkait secara aman (Eager Loading)
            var currentUser = await _userManager.Users
                .Include(u => u.Karyawan)
                .FirstOrDefaultAsync(u => u.UserName == User.Identity.Name);

            var userRoles = currentUser != null 
                ? await _userManager.GetRolesAsync(currentUser) 
                : new List<string>();

            string fotoUrl = !string.IsNullOrEmpty(currentUser?.Karyawan?.UrlFoto)
                ? currentUser.Karyawan.UrlFoto
                : "~/images/no-image.png";

            var model = new UserInfoViewModel
            {
                User = currentUser,
                Roles = userRoles,
                FotoUrl = fotoUrl
            };

            // Mengembalikan view khusus komponen
            return View(model);
        }
    }
}