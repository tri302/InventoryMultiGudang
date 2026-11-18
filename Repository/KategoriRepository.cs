using InventoryMultiGudang.Models;
using InventoryMultiGudang.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace InventoryMultiGudang.Repository
{

    /// <summary>
    /// Implementasi Repository untuk Pengolahan Data Kategori di Database
    /// </summary>
    public class KategoriRepository : IKategoriRepository
    {
        private readonly ApplicationDbContext _context;

        public KategoriRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Mengambil data kategori berformat paged dengan filter pencarian
        /// </summary>
        public async Task<PagedResultKategori<KategoriViewModel>> GetPagedAsync(string? search, int page, int pageSize)
        {
            var query = _context.Kategoris.AsNoTracking().AsQueryable();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(k => k.NamaKategori.Contains(search) || (k.Deskripsi != null && k.Deskripsi.Contains(search)));
            }

            int totalItems = await query.CountAsync();

            var items = await query
                .OrderByDescending(k => k.Id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(k => new KategoriViewModel
                {
                    Id = k.Id,
                    NamaKategori = k.NamaKategori,
                    Deskripsi = k.Deskripsi
                })
                .ToListAsync();

            return new PagedResultKategori<KategoriViewModel>
            {
                Items = items,
                TotalItems = totalItems,
                PageNumber = page,
                PageSize = pageSize
            };
        }

        /// <summary>
        /// Mengambil detail satu kategori berdasarkan ID
        /// </summary>
        public async Task<KategoriViewModel?> GetByIdAsync(int id)
        {
            var entity = await _context.Kategoris.FindAsync(id);
            if (entity == null) return null;

            return new KategoriViewModel
            {
                Id = entity.Id,
                NamaKategori = entity.NamaKategori,
                Deskripsi = entity.Deskripsi
            };
        }

        /// <summary>
        /// Menambahkan kategori baru ke database
        /// </summary>
        public async Task<bool> CreateAsync(KategoriViewModel vm)
        {
            var entity = new Kategori
            {
                NamaKategori = vm.NamaKategori,
                Deskripsi = vm.Deskripsi
            };

            _context.Kategoris.Add(entity);
            return await _context.SaveChangesAsync() > 0;
        }

        /// <summary>
        /// Memperbarui data kategori di database
        /// </summary>
        public async Task<bool> UpdateAsync(KategoriViewModel vm)
        {
            var entity = await _context.Kategoris.FindAsync(vm.Id);
            if (entity == null) return false;

            entity.NamaKategori = vm.NamaKategori;
            entity.Deskripsi = vm.Deskripsi;

            _context.Kategoris.Update(entity);
            return await _context.SaveChangesAsync() > 0;
        }

        /// <summary>
        /// Menghapus data kategori dari database berdasarkan ID
        /// </summary>
        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _context.Kategoris.FindAsync(id);
            if (entity == null) return false;

            _context.Kategoris.Remove(entity);
            return await _context.SaveChangesAsync() > 0;
        }

        /// <summary>
        /// Memeriksa duplikasi Nama Kategori (Unique constraint validation)
        /// </summary>
        public async Task<bool> IsNamaKategoriExistsAsync(string namaKategori, int excludeId = 0)
        {
            return await _context.Kategoris.AnyAsync(k => k.NamaKategori.ToLower() == namaKategori.ToLower() && k.Id != excludeId);
        }
    }
}
