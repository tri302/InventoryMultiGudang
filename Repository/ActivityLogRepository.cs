using Microsoft.EntityFrameworkCore;
using InventoryMultiGudang.ViewModels;
using InventoryMultiGudang.Models;

namespace InventoryMultiGudang.Repository
{
    public class ActivityLogRepository : IActivityLogRepository
    {
        private readonly ApplicationDbContext _context;

        public ActivityLogRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Mengambil data log dengan sistem paging dan pencarian optimal.
        /// </summary>
        public async Task<PagedResultViewModel<ActivityLogViewModel>> GetPagedLogsAsync(string searchQuery, int pageNumber, int pageSize)
        {
            var query = _context.ActivityLogs.AsNoTracking();

            // Filter pencarian berdasarkan Causer, Event, Description, atau Subject (Tabel)
            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                searchQuery = searchQuery.Trim().ToLower();
                query = query.Where(x => x.Causer.ToLower().Contains(searchQuery) ||
                                         x.Event.ToLower().Contains(searchQuery) ||
                                         x.Description.ToLower().Contains(searchQuery) ||
                                         x.Subject.ToLower().Contains(searchQuery));
            }

            int totalItems = await query.CountAsync();
            int totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);
            
            if (pageNumber < 1) pageNumber = 1;
            if (pageNumber > totalPages && totalPages > 0) pageNumber = totalPages;

            int skip = (pageNumber - 1) * pageSize;

            var dataList = await query
                .OrderByDescending(x => x.Timestamp)
                .Skip(skip)
                .Take(pageSize)
                .Select(x => new ActivityLogViewModel
                {
                    Id = x.Id,
                    Causer = x.Causer,
                    Event = x.Event,
                    Description = x.Description,
                    Subject = x.Subject,
                    TimeFormatted = x.Timestamp.ToString("dd MMM yyyy, HH:mm:ss")
                })
                .ToListAsync();

            int firstItemIndex = totalItems == 0 ? 0 : skip + 1;
            int lastItemIndex = Math.Min(skip + pageSize, totalItems);

            return new PagedResultViewModel<ActivityLogViewModel>
            {
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalItems = totalItems,
                TotalPages = totalPages,
                FirstItemIndex = firstItemIndex,
                LastItemIndex = lastItemIndex,
                Items = dataList
            };
        }

        public async Task<ActivityLogViewModel> GetByIdAsync(long id)
        {
            return await _context.ActivityLogs
                .Where(x => x.Id == id)
                .Select(x => new ActivityLogViewModel
                {
                    Id = x.Id,
                    Causer = x.Causer,
                    Event = x.Event,
                    Description = x.Description,
                    Subject = x.Subject,
                    TimeFormatted = x.Timestamp.ToString("dd MMM yyyy, HH:mm:ss")
                })
                .FirstOrDefaultAsync();
        }
    }
}