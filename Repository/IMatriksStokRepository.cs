using InventoryMultiGudang.Models;
using InventoryMultiGudang.ViewModels;

namespace InventoryMultiGudang.Repository
{
    public interface IMatriksStokRepository
    {
        /// <summary>
        /// Mengambil daftar gudang dengan dukungan pencarian (live search) dan limitasi data.
        /// </summary>
        Task<IEnumerable<WarehouseHeaderDto>> GetAllWarehousesAsync(string searchTerm = null, int limit = 10);

        /// <summary>
        /// Mengambil data produk dan matriks stok cross-tab antar gudang secara pagination menggunakan EF Core.
        /// </summary>
        Task<(IEnumerable<ProductStockRowDto> Products, int TotalCount)> GetMatriksStokDataAsync(int? gudangId, int pageNumber, int pageSize);
        Task<bool> UpdateSafetyStockAsync(int productId, int warehouseId, int newSafetyStock);              
        Task<MatriksStokReportVm> GetLaporanMatriksStokAsync(string? penyerah);  
        Task<string> GetNamaPenyerahAsync(string? loggedInUser);
    }
}
