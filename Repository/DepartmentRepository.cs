using InventoryMultiGudang.Models;
using InventoryMultiGudang.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace InventoryMultiGudang.Repository
{
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly ApplicationDbContext _context;

        public DepartmentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResultDepartment<DepartmentListViewModel>> GetPagedAsync(string? search, int pageNumber, int pageSize)
        {
            var query = _context.Departments.AsNoTracking().AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim().ToLower();
                query = query.Where(d => d.Code.ToLower().Contains(search) || d.Name.ToLower().Contains(search));
            }

            var totalItems = await query.CountAsync();
            var items = await query
                .OrderByDescending(d => d.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(d => new DepartmentListViewModel
                {
                    Id = d.Id,
                    Code = d.Code,
                    Name = d.Name,
                    Description = d.Description
                })
                .ToListAsync();

            return new PagedResultDepartment<DepartmentListViewModel>
            {
                Items = items,
                TotalItems = totalItems,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<Department?> GetByIdAsync(int id)
        {
            return await _context.Departments.FindAsync(id);
        }

        public async Task<bool> ExistsCodeAsync(string code, int? excludeId = null)
        {
            return await _context.Departments
                .AnyAsync(d => d.Code.ToLower() == code.ToLower() && (!excludeId.HasValue || d.Id != excludeId.Value));
        }

        public async Task AddAsync(Department department)
        {
            await _context.Departments.AddAsync(department);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Department department)
        {
            _context.Departments.Update(department);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var dept = await _context.Departments.FindAsync(id);
            if (dept != null)
            {
                _context.Departments.Remove(dept);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<string> GenerateDepartmentCodeAsync()
        {
            var maxId = await _context.Departments.MaxAsync(d => (int?)d.Id) ?? 0;
            return $"DPT-{(maxId + 1):D3}";
        }
    }
}
