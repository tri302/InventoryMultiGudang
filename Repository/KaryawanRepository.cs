using InventoryMultiGudang.Models;
using InventoryMultiGudang.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace InventoryMultiGudang.Repository
{
    public class KaryawanRepository : IKaryawanRepository
    {
        private readonly ApplicationDbContext _context;

        public KaryawanRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResultKaryawan<Karyawan>> GetPagedAsync(KaryawanFilterViewModel filter)
        {
            // Gunakan AsNoTracking() untuk query baca murni
            var query = _context.Karyawans.AsNoTracking().AsQueryable();

            // 1. Filter Search Term
            if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
            {
                var term = filter.SearchTerm.Trim().ToLower();
                query = query.Where(k => (k.Nik != null && k.Nik.ToLower().Contains(term)) ||
                                         (k.NamaLengkap != null && k.NamaLengkap.ToLower().Contains(term)) ||
                                         (k.Email != null && k.Email.ToLower().Contains(term)));
            }

            // 2. Filter Department (Hanya jika diisi & > 0)
            if (filter.DepartmentId.HasValue && filter.DepartmentId > 0)
            {
                query = query.Where(k => k.DepartmentId == filter.DepartmentId.Value);
            }

            // 3. Filter Status (Hanya jika diisi & bukan 'Semua Status')
            if (!string.IsNullOrWhiteSpace(filter.Status) &&
                !filter.Status.Equals("Semua Status", StringComparison.OrdinalIgnoreCase) &&
                !filter.Status.Equals("undefined", StringComparison.OrdinalIgnoreCase))
            {
                var statusVal = filter.Status.Trim().ToLower();
                query = query.Where(k => k.Status != null && k.Status.ToLower() == statusVal);
            }

            // Hitung total data yang lolos filter
            int totalItems = await query.CountAsync();

            int pageNumber = filter.PageNumber < 1 ? 1 : filter.PageNumber;
            int pageSize = filter.PageSize < 1 ? 5 : filter.PageSize;

            // AMBIL DATA + INCLUDE DEPARTMENT DI SINI (SETELAH PAGINATION FIX)
            var items = await query.Include(k => k.Department)
                                   .OrderByDescending(k => k.Id)
                                   .Skip((pageNumber - 1) * pageSize)
                                   .Take(pageSize)
                                   .ToListAsync();

            return new PagedResultKaryawan<Karyawan>
            {
                Items = items,
                TotalItems = totalItems,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public async Task<Karyawan?> GetByIdAsync(int id)
        {
            return await _context.Karyawans.Include(k => k.Department).FirstOrDefaultAsync(k => k.Id == id);
        }

        public async Task<bool> IsNikExistsAsync(string nik, int excludeId = 0)
        {
            return await _context.Karyawans.AnyAsync(k => k.Nik == nik && k.Id != excludeId);
        }

        public async Task AddAsync(Karyawan karyawan)
        {
            await _context.Karyawans.AddAsync(karyawan);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Karyawan karyawan)
        {
            _context.Karyawans.Update(karyawan);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var data = await _context.Karyawans.FindAsync(id);
            if (data != null)
            {
                _context.Karyawans.Remove(data);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Department>> GetDepartmentsAsync()
        {
            return await _context.Departments.ToListAsync();
        }
        public async Task<(IEnumerable<Department> Items, int TotalCount)> GetPagedDepartmentsAsync(string searchTerm, int pageNumber, int pageSize)
        {
            var query = _context.Departments.AsNoTracking().AsQueryable();

            // Filter berdasarkan kata kunci pencarian
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                query = query.Where(d => d.Name.Contains(searchTerm));
            }

            // Hitung total data sebelum dilakukan skip & take
            var totalCount = await query.CountAsync();

            // Ambil data sesuai halaman dan limit 20
            var items = await query
                .OrderBy(d => d.Name)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }

        public async Task<string> GenerateNextNikAsync()
        {
            // Cari data karyawan terakhir yang NIK-nya berawalan "EMP-"
            var lastKaryawan = await _context.Karyawans
                .AsNoTracking()
                .Where(k => k.Nik != null && k.Nik.StartsWith("EMP-"))
                .OrderByDescending(k => k.Id)
                .FirstOrDefaultAsync();

            int nextNumber = 1;
            if (lastKaryawan != null && lastKaryawan.Nik.Length > 4)
            {
                // Ambil angka di belakang "EMP-"
                string numberPart = lastKaryawan.Nik.Substring(4);
                if (int.TryParse(numberPart, out int parsedNum))
                {
                    nextNumber = parsedNum + 1;
                }
            }

            // Format menjadi 3 digit angka berurutan (contoh: EMP-001, EMP-002, dst.)
            return $"EMP-{nextNumber:D3}";
        }
    
    }
}