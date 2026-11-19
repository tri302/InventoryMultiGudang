using InventoryMultiGudang.ViewModels;

public interface ILaporanStockOpnameRepository
{
    Task<StockOpnameSummaryViewModel> GetSummaryReportDataAsync(DateTime? startDate, DateTime? endDate, int? gudangId, int? status);
    Task<IEnumerable<StockOpnameListDto>> GetDaftarSesiOpnameAsync(DateTime? startDate, DateTime? endDate, int? gudangId, int? status);
    Task<IEnumerable<RekapitulasiBarangDto>> GetRekapitulasiBarangAsync(DateTime? startDate, DateTime? endDate, int? gudangId, int? status);
}