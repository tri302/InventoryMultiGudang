using InventoryMultiGudang.Models;

namespace InventoryMultiGudang.Repository
{
    public interface IReparasiAsetsRepository
    {
        Task<(IEnumerable<ReparasiAssetViewModel> Items, int TotalCount)> GetReparasiListAsync(string search, int page, int pageSize);        
        Task<object> GetReparasiByIdAsync(int id);
        Task<bool> SaveOrUpdateReparasiAsync(ReparasiAsset model);
        Task<bool> DeleteReparasiAsync(int id);
        Task<(IEnumerable<object> Items, bool More)> GetProdukItemSelect2Async(string search, int page, int pageSize);
    }
}