using InventoryMultiGudang.Models;
using InventoryMultiGudang.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace InventoryMultiGudang.Repository
{
    public class RakRepository : IRakRepository
    {
        private readonly ApplicationDbContext _context;

        public RakRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResultRak<RakViewModel>> GetPagedAsync(int pageIndex, int pageSize)
        {
            var query = _context.Raks
                .Include(r => r.Gudang) // Join dengan Gudang jika relasi navigation property tersedia
                .AsNoTracking()
                .OrderByDescending(r => r.Id);

            var totalItems = await query.CountAsync();

            var items = await query
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .Select(r => new RakViewModel
                {
                    Id = r.Id,
                    Area = r.Area,
                    NamaRak = r.NamaRak,
                    BinNo = r.BinNo,
                    KodeLokasi = r.KodeLokasi,
                    Deskripsi = r.Deskripsi,
                    IsActive = r.IsActive,
                    GudangId = r.GudangId,
                    NamaGudang = r.Gudang != null ? r.Gudang.NamaGudang : "-"
                })
                .ToListAsync();

            return new PagedResultRak<RakViewModel>
            {
                Items = items,
                TotalItems = totalItems,
                PageIndex = pageIndex,
                PageSize = pageSize
            };
        }

        public async Task<RakViewModel?> GetByIdAsync(int id)
        {
            var rak = await _context.Raks
                .Include(r => r.Gudang)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (rak == null) return null;

            return new RakViewModel
            {
                Id = rak.Id,
                Area = rak.Area,
                NamaRak = rak.NamaRak,
                BinNo = rak.BinNo,
                KodeLokasi = rak.KodeLokasi,
                Deskripsi = rak.Deskripsi,
                IsActive = rak.IsActive,
                GudangId = rak.GudangId,
                NamaGudang = rak.Gudang != null ? rak.Gudang.NamaGudang : null
            };
        }

        public async Task<bool> IsKodeLokasiExistsAsync(string kodeLokasi, int excludeId = 0)
        {
            return await _context.Raks
                .AnyAsync(r => r.KodeLokasi.ToLower() == kodeLokasi.ToLower() && r.Id != excludeId);
        }

        public async Task CreateAsync(RakViewModel model)
        {
            var entity = new Rak
            {
                Area = model.Area,
                NamaRak = model.NamaRak,
                BinNo = model.BinNo,
                KodeLokasi = model.KodeLokasi,
                Deskripsi = model.Deskripsi,
                IsActive = model.IsActive,
                GudangId = model.GudangId // Simpan GudangId
            };

            await _context.Raks.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(RakViewModel model)
        {
            var entity = await _context.Raks.FindAsync(model.Id);
            if (entity != null)
            {
                entity.Area = model.Area;
                entity.NamaRak = model.NamaRak;
                entity.BinNo = model.BinNo;
                entity.KodeLokasi = model.KodeLokasi;
                entity.Deskripsi = model.Deskripsi;
                entity.IsActive = model.IsActive;
                entity.GudangId = model.GudangId; // Update GudangId

                _context.Raks.Update(entity);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _context.Raks.FindAsync(id);
            if (entity != null)
            {
                _context.Raks.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Select2PagedResult> GetGudangOptionsAsync(string? search, int page = 1)
        {
            int pageSize = 10; // Membatasi data maksimal 10 per request

            var query = _context.Gudangs
                .Where(x => !x.IsAudit && x.NamaGudang.ToLower() != "gudang transit virtual")
                .AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(g => g.NamaGudang.Contains(search) || g.KodeGudang.Contains(search));
            }

            var totalItems = await query.CountAsync();
            var items = await query
                .OrderBy(g => g.NamaGudang)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(g => new
                {
                    id = g.Id,
                    text = g.NamaGudang + " (" + g.KodeGudang + ")"
                })
                .ToListAsync();

            return new Select2PagedResult
            {
                Results = items,
                Pagination = new Select2Pagination
                {
                    More = (page * pageSize) < totalItems
                }
            };
        }
    }
}