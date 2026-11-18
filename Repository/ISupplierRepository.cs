using InventoryMultiGudang.Models;
using InventoryMultiGudang.ViewModels;

namespace InventoryMultiGudang.Repository
{

    public interface ISupplierRepository
    {
        Task<SupplierPagedResultViewModel> GetPagedAsync(string search, bool? status, int page, int pageSize);
        Task<Supplier> GetByIdAsync(int id);
        Task<string> GenerateKodeSupplierAsync();
        Task AddAsync(Supplier supplier);
        Task UpdateAsync(Supplier supplier);
        Task DeleteAsync(int id);
    }
}

