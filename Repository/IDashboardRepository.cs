using InventoryMultiGudang.ViewModels;

namespace InventoryMultiGudang.Repository
{
    public interface IDashboardRepository
    {
        Task<decimal> GetTotalValuasiFinansialAsync(int? gudangId);
        Task<int> GetTotalUnitAsetSnAsync(int? gudangId);
        Task<int> GetTotalVarianSkuAsync(int? gudangId);
        Task<int> GetTotalStokFisikAsync(int? gudangId);
        Task<int> GetTotalGudangAktifAsync(int? gudangId);
        Task<List<GudangDropdownViewModel>> GetListGudangAsync();
        Task<int> GetTotalVarianHabisAsync(int? gudangId);
        Task<List<AktivitasMutasiViewModel>> GetListAktivitasMutasiAsync(int? gudangId, int takeCount = 10);
        Task<int> GetTotalAktivitasMutasiAsync(int? gudangId);
        Task<List<AktivitasMutasiViewModel>> GetPagedAktivitasMutasiAsync(int? gudangId, int pageNumber, int pageSize);
        Task<GrafikMutasiHarianViewModel> GetGrafikMutasiHarianAsync(int? gudangId, DateTime tglMulai, DateTime tglAkhir);
        Task<GrafikStatusAsetViewModel> GetGrafikStatusAsetAsync(int? gudangId);
        Task<List<DistribusiKategoriViewModel>> GetDistribusiKategoriAsync(int? gudangId);
        Task<List<ValuasiGudangViewModel>> GetValuasiPerGudangAsync(int? gudangId);
        Task<List<PenguasaanDepartemenViewModel>> GetPenguasaanPerDepartemenAsync(int? gudangId);
        Task<KomposisiValuasiTipeViewModel> GetKomposisiValuasiTipeAsync(int? gudangId);
        Task<List<OverdueLoanViewModel>> GetOverdueLoansAsync(int? gudangId);
        Task<List<StokKritisViewModel>> GetStokKritisListAsync(int? gudangId);
    }
}