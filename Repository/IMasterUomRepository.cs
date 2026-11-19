using InventoryMultiGudang.Models;
using Microsoft.EntityFrameworkCore;

namespace InventoryMultiGudang.Repository
{
    public interface IMasterUomRepository
    {
        Task<(IEnumerable<UnitOfMeasure> Data, int TotalCount)> GetPagedAsync(string searchKeyword, int pageIndex, int pageSize);
        Task<UnitOfMeasure?> GetByIdAsync(int id);
        Task<bool> IsCodeExistsAsync(string code, int excludeId = 0);
        Task<bool> AddAsync(UnitOfMeasure uom);
        Task<bool> UpdateAsync(UnitOfMeasure uom);
        Task<bool> DeleteAsync(int id);
    }
}