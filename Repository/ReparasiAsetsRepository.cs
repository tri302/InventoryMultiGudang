using Microsoft.EntityFrameworkCore;
using InventoryMultiGudang.Models;
namespace InventoryMultiGudang.Repository
{
    public class ReparasiAsetsRepository : IReparasiAsetsRepository
    {
        private readonly ApplicationDbContext _context;

        public ReparasiAsetsRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<(IEnumerable<ReparasiAssetViewModel> Items, int TotalCount)> GetReparasiListAsync(string search, int page, int pageSize)
        {
            var query = from r in _context.ReparasiAssets
                        join pi in _context.ProdukItems on r.ProdukItemId equals pi.Id
                        join p in _context.Produks on pi.ProdukId equals p.Id
                        select new ReparasiAssetViewModel
                        {
                            Id = r.Id,
                            ProdukItemId = r.ProdukItemId,
                            SerialNumber = pi.SerialNumber,
                            NamaBarang = p.NamaProduk,
                            TanggalMasuk = r.TanggalMasuk.ToString("yyyy-MM-dd"),
                            TanggalKeluar = r.TanggalKeluar.HasValue ? r.TanggalKeluar.Value.ToString("yyyy-MM-dd") : "-",
                            Keluhan = r.Keluhan,
                            Biaya = r.Biaya,
                            StatusReparasi = r.StatusReparasi,
                            StatusLabel = r.StatusReparasi == 0 ? "Proses" : (r.StatusReparasi == 1 ? "Selesai" : "Dibatalkan")
                        };

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(x => x.SerialNumber.Contains(search) || x.NamaBarang.Contains(search) || x.Keluhan.Contains(search));
            }

            int totalCount = await query.CountAsync();
            var items = await query.OrderByDescending(x => x.Id).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            return (items, totalCount);
        }

        public async Task<object> GetReparasiByIdAsync(int id)
        {
            var data = await (from r in _context.ReparasiAssets
                              join pi in _context.ProdukItems on r.ProdukItemId equals pi.Id
                              join p in _context.Produks on pi.ProdukId equals p.Id
                              where r.Id == id
                              select new
                              {
                                  id = r.Id,
                                  produkItemId = r.ProdukItemId,
                                  serialNumber = pi.SerialNumber,
                                  namaBarang = p.NamaProduk,
                                  tanggalMasuk = r.TanggalMasuk,
                                  tanggalKeluar = r.TanggalKeluar,
                                  keluhan = r.Keluhan,
                                  biaya = r.Biaya,
                                  statusReparasi = r.StatusReparasi
                              }).FirstOrDefaultAsync();

            return data; // Kembalikan datanya saja (bisa null jika tidak ditemukan)
        }

        public async Task<bool> SaveOrUpdateReparasiAsync(ReparasiAsset model)
        {
            // 1. Ambil data produk_item yang terkait dengan reparasi ini
            var produkItem = await _context.ProdukItems.FindAsync(model.ProdukItemId);

            if (produkItem != null)
            {
                // 2. Tentukan status produk_item berdasarkan StatusReparasi
                // StatusReparasi: 0 = Proses (Repair), 1 = Selesai (In Stock), 2 = Dibatalkan
                if (model.StatusReparasi == 0) // Proses / Sedang Reparasi
                {
                    produkItem.Status = "Repair";
                }
                else if (model.StatusReparasi == 1) // Selesai
                {
                    produkItem.Status = "In Stock";
                }
                else if (model.StatusReparasi == 2) // Dibatalkan (opsional: kembalikan ke In Stock atau biarkan)
                {
                    produkItem.Status = "In Stock";
                }

                _context.ProdukItems.Update(produkItem);
            }

            // 3. Simpan atau Update data Reparasi Asset
            if (model.Id == 0)
            {
                _context.ReparasiAssets.Add(model);
            }
            else
            {
                _context.ReparasiAssets.Update(model);
            }

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteReparasiAsync(int id)
        {
            var data = await _context.ReparasiAssets.FindAsync(id);
            if (data == null) return false;

            _context.ReparasiAssets.Remove(data);
            return await _context.SaveChangesAsync() > 0;
        }

        // Implementasi untuk Select2 Aset (Limit 5 data + Live Search)
        public async Task<(IEnumerable<object> Items, bool More)> GetProdukItemSelect2Async(string search, int page, int pageSize)
        {
            var query = from pi in _context.ProdukItems
                        join p in _context.Produks on pi.ProdukId equals p.Id
                        select new
                        {
                            id = pi.Id,
                            text = pi.SerialNumber + " - " + p.NamaProduk
                        };

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(x => x.text.Contains(search));
            }

            // Ambil data dengan skip & take, ambil pageSize + 1 untuk cek apakah ada halaman berikutnya
            var items = await query
                .OrderBy(x => x.id)
                .Skip((page - 1) * pageSize)
                .Take(pageSize + 1)
                .ToListAsync();

            bool more = items.Count > pageSize;
            if (more)
            {
                items.RemoveAt(pageSize); // Buang kelebihan 1 data pengecekan
            }

            return (items, more);
        }
    }
}