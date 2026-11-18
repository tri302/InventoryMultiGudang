using System.ComponentModel.DataAnnotations;

namespace InventoryMultiGudang.ViewModels
{
    // ViewModel untuk Form Pilihan Target Gudang Modal
    public class TargetAuditViewModel
    {
        [Required(ErrorMessage = "Gudang target audit wajib dipilih!")]
        public int? GudangId { get; set; }
    }
}
