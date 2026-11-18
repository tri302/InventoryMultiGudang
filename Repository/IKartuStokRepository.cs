using InventoryMultiGudang.ViewModels;

namespace InventoryMultiGudang.Repository
{
    public interface IKartuStokRepository
    {
        Task<IReadOnlyList<SelectOptionViewModel>> SearchGudangAsync(
            string? keyword,
            CancellationToken cancellationToken = default);

        Task<KartuStokDetailViewModel?> GetDetailsAsync(
            KartuStokFilterViewModel filter,
            CancellationToken cancellationToken = default);
        
        // Mengambil seluruh data kartu stok untuk kebutuhan report/cetak.
        Task<KartuStokReportViewModel?> GetReportKartuStokAsync(int gudangId,int produkId, string? penyerah = null);
        Task<string> GetNamaPenyerahAsync(string? loggedInUser);
        /// <summary>
        /// Mengambil maksimal 5 gudang untuk initial dropdown.
        /// </summary>
        Task<List<SelectOptionViewModel>> GetInitialGudangAsync();

        /// <summary>
        /// Melakukan live search gudang berdasarkan keyword.
        /// </summary>
        Task<List<SelectOptionViewModel>> SearchGudangAsync(
            string term,
            int limit = 5);
        /// <summary>
        /// Mengambil maksimal 5 produk untuk initial dropdown.
        /// </summary>
        Task<List<SelectOptionViewModel>> GetInitialProdukAsync();

        /// <summary>
        /// Melakukan live search produk berdasarkan keyword.
        /// </summary>
        Task<List<SelectOptionViewModel>> SearchProdukAsync(
            string term,
            int limit = 5,
            int gudangId = 0);
        Task<KartuStokDetailViewModel?> GetReportAsync(int gudangId, int produkId, CancellationToken cancellationToken);       
    }
}
