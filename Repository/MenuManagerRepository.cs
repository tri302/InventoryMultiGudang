using System.Linq.Expressions;
using InventoryMultiGudang.Models;
using Microsoft.EntityFrameworkCore;

namespace InventoryMultiGudang.Repository
{
    public class MenuManagerRepository<T> : IMenuManagerRepository<T> where T : class
    {
        protected readonly ApplicationDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public MenuManagerRepository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task AddAsync(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public void Update(T entity)
        {
            _dbSet.Update(entity);
        }

        public async Task DeleteAsync(T entity)
        {
            _dbSet.Remove(entity);
        }

        public async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate)
        {
            return await _dbSet.AnyAsync(predicate);
        }

        /// <summary>
        /// Mengambil daftar menu berdasarkan Group Name secara async
        /// </summary>
        public async Task<IEnumerable<MenuModel>> GetMenusByGroupAsync(string groupName)
        {
            return await _context.Menus
                .Where(m => m.GroupName == groupName)
                .OrderBy(m => m.SortOrder)
                .ToListAsync();
        }

        /// <summary>
        /// Mendapatkan nilai sort order maksimal pada group tertentu untuk kebutuhan auto-increment urutan
        /// </summary>
        public async Task<int> GetMaxSortOrderAsync(string groupName)
        {
            var maxOrder = await _context.Menus
                .Where(m => m.GroupName == groupName)
                .MaxAsync(m => (int?)m.SortOrder) ?? 0;
            return maxOrder + 1;
        }

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        /// <summary>
        /// Mengambil daftar semua nama group secara unik/distinct dari tabel menus.
        /// </summary>
        public async Task<IEnumerable<string>> GetDistinctGroupsAsync()
        {
            return await _context.Menus
                .Select(m => m.GroupName)
                .Distinct()
                .OrderBy(g => g)
                .ToListAsync();
        }

    }
}