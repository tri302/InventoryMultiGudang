using InventoryMultiGudang.Models;
using InventoryMultiGudang.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace InventoryMultiGudang.Repository
{
    public class GudangRepository : IGudangRepository
    {
        private readonly ApplicationDbContext _context;

        public GudangRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResultGudang<GudangViewModel>> GetPagedAsync(int pageIndex, int pageSize, string? search, string? tipe)
        {
            var query = _context.Gudangs.AsNoTracking().AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                search = search.Trim().ToLower();
                query = query.Where(g => g.KodeGudang.ToLower().Contains(search) ||
                                         g.NamaGudang.ToLower().Contains(search) ||
                                         g.PicGudang.ToLower().Contains(search));
            }

            if (!string.IsNullOrWhiteSpace(tipe) && tipe.ToLower() != "semua tipe")
            {
                query = query.Where(g => g.TipeGudang.ToLower() == tipe.ToLower());
            }

            var totalItems = await query.CountAsync();

            // Menggunakan GroupJoin agar dieksekusi menjadi 1 kali query SQL (LEFT JOIN + GROUP BY)
            var items = await query
                .OrderByDescending(g => g.Id)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .GroupJoin(
                    _context.Produks.AsNoTracking(),
                    g => g.Id,
                    p => p.GudangId,
                    (g, produks) => new GudangViewModel
                    {
                        Id = g.Id,
                        KodeGudang = g.KodeGudang,
                        NamaGudang = g.NamaGudang,
                        TipeGudang = g.TipeGudang,
                        PicGudang = g.PicGudang,
                        AlamatGudang = g.AlamatGudang,
                        IsActive = g.IsActive,
                        TotalProduk = produks.Count(),
                        TotalStokBarang = produks.Sum(p => (int?)p.Stok) ?? 0
                    }
                )
                .ToListAsync();

            return new PagedResultGudang<GudangViewModel>
            {
                Items = items,
                TotalItems = totalItems,
                PageIndex = pageIndex,
                PageSize = pageSize
            };
        }

        public async Task<GudangViewModel?> GetByIdAsync(int id)
        {
            var g = await _context.Gudangs.FindAsync(id);
            if (g == null) return null;

            return new GudangViewModel
            {
                Id = g.Id,
                KodeGudang = g.KodeGudang,
                NamaGudang = g.NamaGudang,
                TipeGudang = g.TipeGudang,
                PicGudang = g.PicGudang,
                AlamatGudang = g.AlamatGudang,
                IsActive = g.IsActive,
                TotalProduk = g.TotalProduk,
                TotalStokBarang = g.TotalStokBarang
            };
        }

        public async Task<bool> IsKodeExistsAsync(string kode, int? excludeId = null)
        {
            return await _context.Gudangs
                .AnyAsync(g => g.KodeGudang == kode && (!excludeId.HasValue || g.Id != excludeId.Value));
        }

        public async Task CreateAsync(GudangViewModel vm)
        {
            // 1. Generate Kode Gudang otomatis berdasarkan Nama Gudang
            string subCode = "UMUM";
            if (!string.IsNullOrWhiteSpace(vm.NamaGudang))
            {
                var words = vm.NamaGudang.Trim().Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (words.Length > 0)
                {
                    var lastWord = words.Last().ToUpper();
                    subCode = lastWord.Length >= 3 ? lastWord.Substring(0, 3) : lastWord;
                }
            }

            string prefix = $"GDG-{subCode}-";

            // Cari kode terakhir dengan awalan prefix yang sama untuk menentukan nomor urut selanjutnya
            var lastGudang = await _context.Gudangs
                .Where(g => g.KodeGudang.StartsWith(prefix))
                .OrderByDescending(g => g.Id)
                .FirstOrDefaultAsync();

            int nextNumber = 1;
            if (lastGudang != null && !string.IsNullOrEmpty(lastGudang.KodeGudang))
            {
                var parts = lastGudang.KodeGudang.Split('-');
                if (parts.Length > 2 && int.TryParse(parts.Last(), out int lastNum))
                {
                    nextNumber = lastNum + 1;
                }
            }

            // Hasil akhir otomatis (contoh: GDG-JKT-1, GDG-JKT-2, dst)
            string generatedKodeGudang = $"{prefix}{nextNumber}";

            // 2. Simpan ke database
            var entity = new Gudang
            {
                KodeGudang = generatedKodeGudang, 
                NamaGudang = vm.NamaGudang,
                TipeGudang = vm.TipeGudang,
                PicGudang = vm.PicGudang,
                AlamatGudang = vm.AlamatGudang,
                IsActive = vm.IsActive,
                CreatedAt = DateTime.Now
            };

            _context.Gudangs.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(GudangViewModel vm)
        {
            var entity = await _context.Gudangs.FindAsync(vm.Id);
            if (entity != null)
            {                
                entity.NamaGudang = vm.NamaGudang;
                entity.TipeGudang = vm.TipeGudang;
                entity.PicGudang = vm.PicGudang;
                entity.AlamatGudang = vm.AlamatGudang;
                entity.IsActive = vm.IsActive;
                entity.UpdatedAt = DateTime.Now;

                _context.Gudangs.Update(entity);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _context.Gudangs.FindAsync(id);
            if (entity != null)
            {
                _context.Gudangs.Remove(entity);
                await _context.SaveChangesAsync();
            }
        }       
    }
}
