using InventoryMultiGudang.Models;

namespace InventoryMultiGudang.ViewModels
{
    public class UserInfoViewModel
    {
        public ApplicationUser? User { get; set; }
        public IList<string> Roles { get; set; } = new List<string>();
        public string FotoUrl { get; set; } = "~/images/default-user.png";
    }
}