using System.ComponentModel.DataAnnotations;
using static InventoryMultiGudang.Helper.UploadFileHelper;

namespace InventoryMultiGudang.ViewModels
{
    public class CustomerViewModel
    {
        public int Id { get; set; }

        public string? KodeCustomer { get; set; }

        [Required(ErrorMessage = "Nama Customer / Instansi wajib diisi!")]
        [StringLength(150, ErrorMessage = "Nama customer maksimal 150 karakter.")]
        public string NamaCustomer { get; set; } = string.Empty;

        [Required(ErrorMessage = "Alamat Email wajib diisi!")]
        [EmailAddress(ErrorMessage = "Format email tidak valid!")]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Nomor Telepon wajib diisi!")]
        [Phone(ErrorMessage = "Format nomor telepon tidak valid!")]
        [StringLength(20)]
        public string NoTelepon { get; set; } = string.Empty;

        [Required(ErrorMessage = "Alamat Site / Kantor wajib diisi!")]
        [StringLength(500)]
        public string Alamat { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;        
    }

    public class CustomerDataTableRequest
    {
        public string? Search { get; set; }
        public string? Status { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }

    public class PaginatedListCustomer<T>
    {
        public List<T> Items { get; set; } = new List<T>();
        public int TotalItems { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalItems / PageSize);
        public bool HasPreviousPage => PageIndex > 1;
        public bool HasNextPage => PageIndex < TotalPages;
    }
}
