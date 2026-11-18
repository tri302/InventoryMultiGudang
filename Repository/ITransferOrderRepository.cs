using InventoryMultiGudang.Models;
using InventoryMultiGudang.ViewModels;

namespace InventoryMultiGudang.Repository
{
    public interface ITransferOrderRepository
    {
        Task<PagedResultTransferOrder<TransferOrder>> GetPagedListAsync(TransferOrderFilterViewModel filter);
        Task<List<Produk>> GetProdukPagedAsync(string? search, int limit = 20, int? gudangId = null);
        //Task<TransferOrder?> GetByIdAsync(int id);
        Task<TransferOrderDetailViewModel?> GetByIdAsync(int id);
        Task<TransferOrderDetailViewModel?> GetByIdAsync(int id, string penyerah);
        Task<bool> CreateAsync(TransferOrderViewModel model);
        Task<bool> UpdateAsync(TransferOrderViewModel model);
        Task<bool> DeleteAsync(int id);
        Task<bool> ReceiveTransferAsync(int id, Dictionary<int, int> receivedQtyMap);
        Task<List<Gudang>> GetGudangPagedAsync(string? search, int limit = 20);
        Task<List<Produk>> GetProdukPagedAsync(string? search, int limit = 20);
        Task<string> GetNamaPenyerahAsync(string? loggedInUser);
    }
}