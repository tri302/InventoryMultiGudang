using Microsoft.AspNetCore.Mvc;
using InventoryMultiGudang.Repository;
using InventoryMultiGudang.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace InventoryMultiGudang.Controllers
{
    // [Authorize(Roles = "Super Admin, Manager, User, Warehouse Manager,Warehouse Staff, Admin")]

    // Menggunakan Policy dinamis yang merujuk ke tabel permissions
    // agar dinamis dan fleksibel sesuai dengan data di database
     // Ganti dengan nama policy yang sesuai
     [Authorize(Policy = "view dashboard")]
    public class DashboardController : Controller
    {
        private readonly IDashboardRepository _dashboardRepository;

        public DashboardController(IDashboardRepository dashboardRepository)
        {
            _dashboardRepository = dashboardRepository;
        }

        /// <summary>
        /// Action utama untuk merender halaman awal Dashboard.
        /// </summary>                
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Mengambil seluruh data statistik, KPI, dan mutasi secara asynchronous dalam format JSON melalui Repository.
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetDashboardData(int? gudangId, DateTime? tglMulai, DateTime? tglAkhir, int page = 1)
        {
            try
            {
                // Default 14 Hari Terakhir jika tanggal tidak diisi
                DateTime endDate = tglAkhir ?? DateTime.Now;
                DateTime startDate = tglMulai ?? endDate.AddDays(-13);

                int pageSize = 10;

                // Panggil Repository
                var totalValuasi = await _dashboardRepository.GetTotalValuasiFinansialAsync(gudangId);
                var totalUnitSn = await _dashboardRepository.GetTotalUnitAsetSnAsync(gudangId);
                var totalSku = await _dashboardRepository.GetTotalVarianSkuAsync(gudangId);
                var totalFisik = await _dashboardRepository.GetTotalStokFisikAsync(gudangId);
                var totalGudang = await _dashboardRepository.GetTotalGudangAktifAsync(gudangId);
                var totalVarianHabis = await _dashboardRepository.GetTotalVarianHabisAsync(gudangId);
                var listGudang = await _dashboardRepository.GetListGudangAsync();
                // Data Grafik
                var grafikMutasi = await _dashboardRepository.GetGrafikMutasiHarianAsync(gudangId, startDate, endDate);
                var grafikStatusAset = await _dashboardRepository.GetGrafikStatusAsetAsync(gudangId);
                // Data Paging Aktivitas Mutasi
                var totalMutasiItems = await _dashboardRepository.GetTotalAktivitasMutasiAsync(gudangId);
                var listMutasi = await _dashboardRepository.GetPagedAktivitasMutasiAsync(gudangId, page, pageSize);
                var valuasiGudang = await _dashboardRepository.GetValuasiPerGudangAsync(gudangId);
                var distribusiKategori = await _dashboardRepository.GetDistribusiKategoriAsync(gudangId);
                var penguasaanDept = await _dashboardRepository.GetPenguasaanPerDepartemenAsync(gudangId);
                var komposisiValuasi = await _dashboardRepository.GetKomposisiValuasiTipeAsync(gudangId);
                var overdueLoans = await _dashboardRepository.GetOverdueLoansAsync(gudangId);
                var stokKritisList = await _dashboardRepository.GetStokKritisListAsync(gudangId);

                var viewModel = new DashboardViewModel
                {
                    TotalValuasiFinansial = totalValuasi,
                    TotalUnitAsetSn = totalUnitSn,
                    TotalVarianSku = totalSku,
                    TotalStokFisik = totalFisik,
                    TotalGudangAktif = totalGudang,

                    TotalAsetDipinjam = grafikStatusAset.OnLoan,
                    TotalAsetCustody = grafikStatusAset.AssignedCustody,
                    GrafikStatusAset = grafikStatusAset,
                    GrafikMutasi = grafikMutasi,

                    ValuasiGudangList = valuasiGudang,
                    DistribusiKategoriList = distribusiKategori,

                    TotalVarianHabis = totalVarianHabis,
                    ListGudang = listGudang,
                    ListAktivitasMutasi = listMutasi,

                    PenguasaanDepartemenList = penguasaanDept,
                    KomposisiValuasiTipe = komposisiValuasi,

                    OverdueLoanList = overdueLoans,
                    StokKritisList = stokKritisList,
                    TotalPeminjamanOverdue = overdueLoans.Count,
                    TotalStokKritis = stokKritisList.Count,

                    CurrentPage = page,
                    PageSize = pageSize,
                    TotalItems = totalMutasiItems,
                    TotalPages = (int)Math.Ceiling(totalMutasiItems / (double)pageSize)
                };

                return Json(new { success = true, data = viewModel });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { success = false, message = "Terjadi kesalahan pada server: " + ex.Message });
            }
        }

    }
}