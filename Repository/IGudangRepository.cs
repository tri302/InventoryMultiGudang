using InventoryMultiGudang.ViewModels;

namespace InventoryMultiGudang.Repository
{
    public interface IGudangRepository
    {
        Task<PagedResultGudang<GudangViewModel>> GetPagedAsync(int pageIndex, int pageSize, string? search, string? tipe);
        Task<GudangViewModel?> GetByIdAsync(int id);
        Task<bool> IsKodeExistsAsync(string kode, int? excludeId = null);
        Task CreateAsync(GudangViewModel vm);
        Task UpdateAsync(GudangViewModel vm);
        Task DeleteAsync(int id);
    }
}
