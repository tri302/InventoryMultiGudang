using InventoryMultiGudang.ViewModels;

namespace InventoryMultiGudang.Repository
{

    /// <summary>
    /// Interface Abstraksi Akses Data Kategori
    /// </summary>
    public interface IKategoriRepository
    {
        Task<PagedResultKategori<KategoriViewModel>> GetPagedAsync(string? search, int page, int pageSize);
        Task<KategoriViewModel?> GetByIdAsync(int id);
        Task<bool> CreateAsync(KategoriViewModel vm);
        Task<bool> UpdateAsync(KategoriViewModel vm);
        Task<bool> DeleteAsync(int id);
        Task<bool> IsNamaKategoriExistsAsync(string namaKategori, int excludeId = 0);
    }
}