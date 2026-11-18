using Microsoft.EntityFrameworkCore;
using InventoryMultiGudang.Models;
using InventoryMultiGudang.Repository;

public class MasterUomRepository : IMasterUomRepository
{
    private readonly ApplicationDbContext _context;
    public MasterUomRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Mengambil data UOM secara paginated dan pencarian secara asynchronous (Optimal & Robust).
    /// </summary>
    public async Task<(IEnumerable<UnitOfMeasure> Data, int TotalCount)> GetPagedAsync(string searchKeyword, int pageIndex, int pageSize)
    {
        var query = _context.MasterUoms.AsNoTracking().AsQueryable();

        if (!string.IsNullOrWhiteSpace(searchKeyword))
        {
            searchKeyword = searchKeyword.Trim().ToLower();
            query = query.Where(x => x.Code.ToLower().Contains(searchKeyword) ||
                                     x.Name.ToLower().Contains(searchKeyword) ||
                                     (x.Description != null && x.Description.ToLower().Contains(searchKeyword)));
        }

        int totalCount = await query.CountAsync();
        var data = await query.OrderBy(x => x.Id)
                              .Skip((pageIndex - 1) * pageSize)
                              .Take(pageSize)
                              .ToListAsync();

        return (data, totalCount);
    }

    /// <summary>
    /// Mengambil data UOM berdasarkan ID.
    /// </summary>
    public async Task<UnitOfMeasure?> GetByIdAsync(int id)
    {
        return await _context.MasterUoms.FindAsync(id);
    }

    /// <summary>
    /// Mengecek apakah Code UOM sudah ada di database untuk mencegah duplikasi.
    /// </summary>
    public async Task<bool> IsCodeExistsAsync(string code, int excludeId = 0)
    {
        return await _context.MasterUoms
            .AnyAsync(x => x.Code.ToLower() == code.ToLower() && x.Id != excludeId);
    }

    /// <summary>
    /// Menambahkan data UOM baru ke database.
    /// </summary>
    public async Task<bool> AddAsync(UnitOfMeasure uom)
    {
        await _context.MasterUoms.AddAsync(uom);
        return await _context.SaveChangesAsync() > 0;
    }

    /// <summary>
    /// Memperbarui data UOM yang sudah ada.
    /// </summary>
    public async Task<bool> UpdateAsync(UnitOfMeasure uom)
    {
        _context.MasterUoms.Update(uom);
        return await _context.SaveChangesAsync() > 0;
    }

    /// <summary>
    /// Menghapus data UOM berdasarkan ID.
    /// </summary>
    public async Task<bool> DeleteAsync(int id)
    {
        var uom = await _context.MasterUoms.FindAsync(id);
        if (uom == null) return false;

        _context.MasterUoms.Remove(uom);
        return await _context.SaveChangesAsync() > 0;
    }
}
