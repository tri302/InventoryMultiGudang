using InventoryMultiGudang.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace InventoryMultiGudang.Repository
{
    public interface IRakRepository
    {
        Task<PagedResultRak<RakViewModel>> GetPagedAsync(int pageIndex, int pageSize);
        Task<RakViewModel?> GetByIdAsync(int id);
        Task<bool> IsKodeLokasiExistsAsync(string kodeLokasi, int excludeId = 0);
        Task CreateAsync(RakViewModel model);
        Task UpdateAsync(RakViewModel model);
        Task DeleteAsync(int id);
        // Return type yang benar untuk Select2
        Task<Select2PagedResult> GetGudangOptionsAsync(string? search, int page = 1);
    }
}
