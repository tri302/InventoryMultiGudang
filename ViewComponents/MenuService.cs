using System.Security.Claims;
using InventoryMultiGudang.Models;
using Microsoft.EntityFrameworkCore;
namespace InventoryMultiGudang.Repository
{
    public interface IMenuService
    {
        Task<List<MenuModel>> GetMenusForUserAsync(ClaimsPrincipal user);
    }

    public class MenuService: IMenuService
    {
        private readonly ApplicationDbContext _db;

        public MenuService(ApplicationDbContext context)
        {
            _db = context;
        }

        public async Task<List<MenuModel>> GetMenusForUserAsync(ClaimsPrincipal user)
        {
            // Jika Super Admin, tampilkan semua menu
            if (user.IsInRole("Super Admin"))
            {
                return await _db.Set<MenuModel>().OrderBy(m => m.SortOrder).ToListAsync();
            }

            // Ambil klaim permission milik user yang sedang login
            var userPermissions = user.Claims
                .Where(c => c.Type == "Permission")
                .Select(c => c.Value)
                .ToList();

            // Filter menu berdasarkan permission yang dimiliki atau jika menu tidak butuh permission
            var menus = await _db.Set<MenuModel>()
                .Where(m => string.IsNullOrEmpty(m.RequirePermissions) || userPermissions.Contains(m.RequirePermissions))
                .OrderBy(m => m.SortOrder)
                .ToListAsync();

            return menus;
        }
    }
}