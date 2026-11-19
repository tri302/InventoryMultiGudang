using InventoryMultiGudang.Models;
using InventoryMultiGudang.ViewModels;

namespace InventoryMultiGudang.Repository
{
    public interface IDepartmentRepository
    {
        Task<PagedResultDepartment<DepartmentListViewModel>> GetPagedAsync(string? search, int pageNumber, int pageSize);
        Task<Department?> GetByIdAsync(int id);
        Task<bool> ExistsCodeAsync(string code, int? excludeId = null);
        Task AddAsync(Department department);
        Task UpdateAsync(Department department);
        Task DeleteAsync(int id);
        Task<string> GenerateDepartmentCodeAsync();
    }
}
