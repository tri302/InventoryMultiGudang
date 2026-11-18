using InventoryMultiGudang.Models;
using InventoryMultiGudang.ViewModels;

namespace InventoryMultiGudang.Repository
{

    public interface IStockOpnameRepository
    {
        Task<(IEnumerable<StockOpnameListViewModel> Data, int TotalCount)> GetPagedAsync(string? search, int? gudangId, string? status, int page, int pageSize);
        Task<StockOpname?> GetByIdAsync(int id);
        Task<StockOpname?> GetByIdAsync(int id, string penyerah);
        Task<IEnumerable<Gudang>> GetAllGudangAsync();
        Task<IEnumerable<Produk>> GetAllProdukAsync();
        Task<IEnumerable<Produk>> GetProdukByGudangIdAsync(int gudangId); 
        Task<StockOpname> CreateDraftAsync(StockOpname entity);
        Task UpdateDraftAsync(StockOpname entity);
        Task<bool> SelesaikanAuditAsync(int id);
        Task<bool> BatalkanAuditAsync(int id);
        Task<bool> DeleteAsync(int id);
        Task<string> GenerateNoReferensiAsync();
        Task<Produk> GetProdukByIdAsync(int produkId);
        Task<string> GetNamaPenyerahAsync(string? loggedInUser);
    }
}
