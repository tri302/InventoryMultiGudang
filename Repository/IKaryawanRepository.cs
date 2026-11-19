using InventoryMultiGudang.Models;
using InventoryMultiGudang.ViewModels;

namespace InventoryMultiGudang.Repository
{
    public interface IKaryawanRepository
    {
        Task<PagedResultKaryawan<Karyawan>> GetPagedAsync(KaryawanFilterViewModel filter);
        Task<Karyawan?> GetByIdAsync(int id);
        Task<bool> IsNikExistsAsync(string nik, int excludeId = 0);
        Task AddAsync(Karyawan karyawan);
        Task UpdateAsync(Karyawan karyawan);
        Task DeleteAsync(int id);
        Task<List<Department>> GetDepartmentsAsync();
        Task<(IEnumerable<Department> Items, int TotalCount)> GetPagedDepartmentsAsync(string searchTerm, int pageNumber, int pageSize);
        Task<string> GenerateNextNikAsync();
    }
}
