using InventoryMultiGudang.ViewModels;

public interface IProdukRepository
{
    Task<PagedResultProduk<ProdukViewModel>> GetPagedAsync(string? search, string? kategori, int page, int pageSize);
    Task AutoGenerateProductItemsAsync(int produkId, int gudangId, int rakId, int quantity, string webRootPath);
    Task<string> GenerateNextSkuAsync(string? kategori);
    Task<ProdukSerialNumbersResultDto?> GetSerialNumbersByProdukIdAsync(int produkId);
    Task<ProdukItemPrintDto?> GetProdukItemForPrintAsync(int produkItemId);
    Task<ProdukViewModel?> GetByIdAsync(int id);
    Task<bool> CreateAsync(ProdukViewModel vm);
    Task<bool> UpdateAsync(ProdukViewModel vm);
    Task<bool> DeleteAsync(int id);
    Task<bool> IsSkuExistsAsync(string sku, int excludeId = 0);
    Task<List<DropdownItemDto>> GetDropdownAsync(string? q);
    Task<object> GetDropdownByGudangAsync(int gudangId, string? q);
    Task<object> GetDropdownKategoriAsync(string? q);
}