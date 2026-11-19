using InventoryMultiGudang.ViewModels;

namespace InventoryMultiGudang.Repository
{
    public interface ILaporanAssetRepository
    {
        Task<(IEnumerable<AssetSerialNumberViewModel> Items, int TotalCount)> GetDaftarSerialNumberAsync(string search, int page, int pageSize);
        Task<(IEnumerable<AssetPeminjamanBelumKembaliViewModel> Items, int TotalCount)> GetPeminjamanBelumKembaliAsync(string search, int page, int pageSize);
        Task<(IEnumerable<AssetPerCustomerViewModel> Items, int TotalCount)> GetAssetPerCustomerAsync(string search, int page, int pageSize);
        Task<IEnumerable<CustomerAssetDetailViewModel>> GetDetailAssetByCustomerAsync(int customerId);
        Task<(IEnumerable<AssetPerDepartemenViewModel> Items, int TotalCount)> GetAssetPerDepartemenAsync(string search, int page, int pageSize);
        Task<IEnumerable<DepartemenAssetDetailViewModel>> GetDetailAssetByDepartemenAsync(int departmentId);
        Task<(IEnumerable<AssetPerKaryawanViewModel> Items, int TotalCount)> GetAssetPerKaryawanAsync(string search, int page, int pageSize);
        Task<IEnumerable<KaryawanAssetDetailViewModel>> GetDetailAssetByKaryawanAsync(int karyawanId);
        Task<LaporanAssetIndexViewModel> GetIndexSummaryAsync();
        Task<AssetTimelineViewModel> GetAssetTimelineBySerialNumberAsync(string serialNumber);        
    }   
}
