using System.Linq.Expressions;
using InventoryMultiGudang.Models;

namespace InventoryMultiGudang.Repository
{
    public interface IMenuManagerRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetByIdAsync(int id);
        Task AddAsync(T entity);
        void Update(T entity);
        Task DeleteAsync(T entity);
        Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate);
        Task<IEnumerable<MenuModel>> GetMenusByGroupAsync(string groupName);
        Task<int> GetMaxSortOrderAsync(string groupName);
        Task<int> CompleteAsync();    
        void Dispose();     
        Task<IEnumerable<string>> GetDistinctGroupsAsync();  
    }
}