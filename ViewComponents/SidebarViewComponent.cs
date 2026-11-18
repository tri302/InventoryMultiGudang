using System.Security.Claims;
using InventoryMultiGudang.Repository;
using Microsoft.AspNetCore.Mvc;

namespace InventoryMultiGudang.ViewComponents
{
    public class SidebarViewComponent : ViewComponent
    {
        private readonly IMenuService _menuService;

        public SidebarViewComponent(IMenuService menuService)
        {
            _menuService = menuService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {            
            var menus = await _menuService.GetMenusForUserAsync((ClaimsPrincipal)User);
            return View(menus);
        }
    }
}