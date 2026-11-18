using InventoryMultiGudang.Models; 
using Microsoft.AspNetCore.Identity;

namespace InventoryMultiGudang.Helper
{
    public static class SeedRolesAsync
    {
        public static async Task SeederRolesAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>(); // Menggunakan ApplicationUser

            // 1. Daftar Role
            string[] roleNames = { "Super Admin", "Admin", "Manager", "Warehouse Manager", "Warehouse Staff", "User" };

            foreach (var roleName in roleNames)
            {
                // Cek apakah role sudah ada
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    // Membuat role jika belum ada
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            // 2. Daftar akun default untuk masing-masing role beserta password-nya
            var defaultUsers = new[]
            {
                new { Email = "superadmin@example.com", Password = "superadmin123", Role = "Super Admin" },
                new { Email = "admin@example.com", Password = "admin123", Role = "Admin" },
                new { Email = "manager@example.com", Password = "manager123", Role = "Manager" },
                new { Email = "warehousemanager@example.com", Password = "warehousemanager123", Role = "Warehouse Manager" },
                new { Email = "warehousestaff@example.com", Password = "warehousestaff123", Role = "Warehouse Staff" },
                new { Email = "user@example.com", Password = "user123", Role = "User" }
            };

            foreach (var item in defaultUsers)
            {
                // Cek apakah user sudah ada berdasarkan email
                var user = await userManager.FindByEmailAsync(item.Email);
                if (user == null)
                {
                    // Inisialisasi menggunakan ApplicationUser
                    var newUser = new ApplicationUser
                    {
                        UserName = item.Email,
                        Email = item.Email,
                        EmailConfirmed = true
                    };

                    // Membuat user dengan password terenkripsi otomatis (PasswordHash)
                    var createResult = await userManager.CreateAsync(newUser, item.Password);
                    if (createResult.Succeeded)
                    {
                        // Menambahkan user ke role yang sesuai
                        await userManager.AddToRoleAsync(newUser, item.Role);
                    }
                }
            }
        }
    }
}