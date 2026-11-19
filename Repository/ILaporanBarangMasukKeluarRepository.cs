using InventoryMultiGudang.ViewModels;

namespace InventoryMultiGudang.Repository
{
    public interface ILaporanBarangMasukKeluarRepository
    {
        Task<LaporanBarangMasukKeluarIndexViewModel> GetIndexDataAsync(string tanggalMulai, string tanggalSelesai, int? gudangId, string tipeTransaksi, int? produkId);
        Task<List<DropdownItemViewModel>> GetGudangDropdownAsync(string searchTerm);        
        Task<List<DropdownItemViewModel>> GetProdukDropdownAsync(string searchTerm, int? gudangId);
        Task<LaporanBarangMasukKeluarIndexViewModel> GetReportDataAsync(string tanggalMulai, string tanggalSelesai, int? gudangId, string tipeTransaksi, int? produkId);
    }
}
