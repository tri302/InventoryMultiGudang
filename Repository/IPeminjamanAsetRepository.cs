using InventoryMultiGudang.Models;
using InventoryMultiGudang.ViewModels;

namespace InventoryMultiGudang.Repository
{
    public interface IPeminjamanAsetRepository
    {
        Task<string> GetNamaPenyerahAsync(string? loggedInUser); 
        Task<(IEnumerable<PeminjamanAsetFormViewModel> Items, int TotalCount)> GetPagedAsync(string search, StatusPeminjaman? status, int pageIndex, int pageSize, int? gudangId);
        Task<PeminjamanAsetFormViewModel> GetByIdAsync(int id, string currentUserName = "Admin");
        Task<IEnumerable<GudangDropdownViewModel>> GetGudangListAsync(string search = null);
        Task<List<AsetAvailableDto>> GetAvailableAssetsAsync(string? search, string? type);
        Task<AsetAvailableDto?> GetAssetBySerialAsync(string serialNumber);
        Task<List<PenerimaSelectOptionDto>> GetPenerimaOptionsAsync(string? search);
        Task<bool> InsertAsync(PeminjamanAsetFormViewModel model);
        Task<bool> ProcessReturnAsync(int id, DateTime tglKembali, List<PeminjamanAsetDetailViewModel> updatedDetails);
        Task<bool> DeleteAsync(int id);
        Task UpdateStatusOtomatisTerlambatAsync(); // Sinkronisasi otomatis status terlambat
    }
}