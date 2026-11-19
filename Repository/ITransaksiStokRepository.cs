using InventoryMultiGudang.Models;
using InventoryMultiGudang.ViewModels;

namespace InventoryMultiGudang.Repository
{
    public interface ITransaksiStokRepository
    {
        Task<(List<TransaksiStok> Data, int TotalCount)> GetPagedAsync(string? search, int? gudangId, string? tipe, int page, int pageSize);
        Task<TransaksiStok?> GetByIdAsync(int id, string penyerah);
        Task<TransaksiStok?> GetByIdAsync(int id);        
        Task<object?> GetDetailKartuStokAsync(int id); 
        Task<bool> CreateAsync(TransaksiStok entity);
        Task<bool> UpdateAsync(TransaksiStok entity);
        Task<bool> DeleteAsync(int id);
        Task<string> GetNamaPenyerahAsync(string? loggedInUser);

        // Dropdown Ajax Search Methods (Limit 20 Items)
        Task<List<DropdownItemDto>> GetDropdownGudangAsync(string? q, int page = 1, int pageSize = 20);
        Task<List<DropdownItemDto>> GetDropdownSupplierAsync(string? q, int page = 1, int pageSize = 20);
        Task<List<DropdownItemDto>> GetDropdownProdukAsync(string? q, int page = 1, int pageSize = 20);
        Task<object> GetDropdownRakByGudangAsync(int gudangId, string? q, int page = 1, int pageSize = 20);
        Task<List<DropdownItemDto>> GetDropdownProdukByGudangAsync(int gudangId, string? q, int page = 1, int pageSize = 20);        
        Task<object?> GetProdukDetailForTransactionAsync(int produkId, int gudangId, string tipeTransaksi, int jumlah = 1);
    }
}