using System.ComponentModel.DataAnnotations;

namespace InventoryMultiGudang.ViewModels
{
    public class TransferOrderViewModel : IValidatableObject
    {
        public int Id { get; set; }

        public string? NomorDokumen { get; set; }

        [Required(ErrorMessage = "Gudang Asal wajib dipilih!")]
        [Display(Name = "Gudang Asal")]
        public int GudangAsalId { get; set; }

        [Required(ErrorMessage = "Gudang Tujuan wajib dipilih!")]
        [Display(Name = "Gudang Tujuan")]
        public int GudangTujuanId { get; set; }

        public string? Catatan { get; set; }

        [Required(ErrorMessage = "Detail produk transfer tidak boleh kosong!")]
        [MinLength(1, ErrorMessage = "Minimal harus menambahkan 1 produk untuk ditransfer.")]
        public List<TransferOrderDetailViewModel> Details { get; set; } = new();

        // Validasi Custom: Gudang asal dan tujuan tidak boleh sama
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (GudangAsalId > 0 && GudangTujuanId > 0 && GudangAsalId == GudangTujuanId)
            {
                yield return new ValidationResult(
                    "Gudang Asal dan Gudang Tujuan tidak boleh sama!",
                    new[] { nameof(GudangTujuanId) }
                );
            }
        }
        public string DibuatOleh { get; set; } = string.Empty;
    }    

    // Helper Filter untuk DataTable / Search Ajax
    public class TransferOrderFilterViewModel
    {
        public string? SearchTerm { get; set; }
        public int? GudangId { get; set; }
        public string? Status { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }

    public class PagedResultTransferOrder<T>
    {
        public List<T> Items { get; set; } = new();
        public int TotalItems { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int PageSize { get; set; }
    }
}
