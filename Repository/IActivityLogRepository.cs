using System.Threading.Tasks;
using InventoryMultiGudang.ViewModels;

namespace InventoryMultiGudang.Repository
{
    public interface IActivityLogRepository
    {
        /// <summary>
        /// Mengambil data activity logs dengan sistem pagination dan pencarian secara async.
        /// </summary>
        Task<PagedResultViewModel<ActivityLogViewModel>> GetPagedLogsAsync(string searchQuery, int pageNumber, int pageSize);

        /// <summary>
        /// Mengambil detail activity log berdasarkan ID.
        /// </summary>
        Task<ActivityLogViewModel> GetByIdAsync(long id);
    }
}